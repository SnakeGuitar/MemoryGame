using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Utilities;
using Client.Views.Controls;
using Client.Views.Session;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool _isHandlingDisconnect = false;
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            string savedLangCode = Client.Properties.Settings.Default.languageCode;

            if (string.IsNullOrEmpty(savedLangCode))
            {
                savedLangCode = "en-US";
            }
            Lang.Culture = new CultureInfo(savedLangCode);

            base.OnStartup(e);

            EventManager.RegisterClassHandler(typeof(Window), Window.LoadedEvent, new RoutedEventHandler(OnWindowLoaded));
            GameServiceManager.Instance.ServerConnectionLost += OnGlobalServerConnectionLost;
            UserServiceManager.Instance.ServerConnectionLost += OnGlobalServerConnectionLost;

            InitializeBackgroundMusic();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GameServiceManager.Instance.ServerConnectionLost -= OnGlobalServerConnectionLost;
            UserServiceManager.Instance.ServerConnectionLost -= OnGlobalServerConnectionLost;
            base.OnExit(e);
        }

        #region Global Disconnect Handler
        private void OnGlobalServerConnectionLost()
        {
            if (_isHandlingDisconnect) return;
            _isHandlingDisconnect = true;

            this.Dispatcher.Invoke(() =>
            {
                string title = Lang.Global_Title_ServerOffline ?? "Server Offline";
                string message = Lang.Global_Error_ConnectionLost ?? "Connection to server lost. Returning to login screen.";

                Window currentOwner = this.MainWindow;
                if (currentOwner != null && !currentOwner.IsVisible) currentOwner = null;

                var msgBox = new CustomMessageBox(
                    title,
                    message,
                    currentOwner,
                    CustomMessageBox.MessageBoxType.Error);

                msgBox.ShowDialog();

                UserSession.EndSession();

                NavigateToLoginCleanly();

                _isHandlingDisconnect = false;
            });
        }

        private void NavigateToLoginCleanly()
        {
            Login loginWindow = new Login();

            Window oldWindow = this.MainWindow;

            this.MainWindow = loginWindow;
            loginWindow.Show();

            if (oldWindow != null)
            {
                oldWindow.Close();
            }
        }

        #endregion

        #region Window Lifecycle Handlers

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Window window)
            {
                WindowHelper.ApplySavedSetting(window);

                window.Closed -= OnWindowClosed;
                window.Closed += OnWindowClosed;
            }
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            if (this.Windows.Count == 0)
            {
                NavigationHelper.ExitApplication();
            }
        }

        #endregion

        #region Exception Handlers

        private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ExceptionManager.Handle(e.Exception, null, null, isFatal: false);
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ExceptionManager.Handle(e.Exception, null, null, isFatal: false);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ExceptionManager.Handle(ex, null, null, isFatal: true);
            }
        }

        #endregion

        #region Music

        private void InitializeBackgroundMusic()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string musicPath = Path.Combine(baseDir, "Resources", "Music");

                if (Directory.Exists(musicPath))
                {
                    var songs = new List<string>(Directory.GetFiles(musicPath, "*.*"));
                    songs = songs.FindAll(s => s.EndsWith(".mp3") || s.EndsWith(".wav"));

                    if (songs.Count > 0)
                    {
                        MusicManager.Instance.Initialize(songs, shuffle: true);
                        MusicManager.Instance.Volume = 0.5;
                    }
                }
                else
                {
                    Directory.CreateDirectory(musicPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading music: {ex.Message}");
            }
        }

        #endregion
    }
}