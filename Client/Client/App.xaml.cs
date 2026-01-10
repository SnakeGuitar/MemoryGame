using Client.Core;
using Client.Helpers;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
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
            Client.Properties.Langs.Lang.Culture = new CultureInfo(savedLangCode);

            base.OnStartup(e);

            EventManager.RegisterClassHandler(typeof(Window), Window.LoadedEvent, new RoutedEventHandler(OnWindowLoaded));
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Window window)
            {
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

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            ExceptionManager.Handle(e.Exception, null, null, isFatal: false);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ExceptionManager.Handle(e.Exception, null, null, isFatal: false);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ExceptionManager.Handle(ex, null, null, isFatal: true);
            }
        }
    }
}