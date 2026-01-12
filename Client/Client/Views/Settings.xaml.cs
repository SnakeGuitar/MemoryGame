using Client.Helpers;
using Client.Properties.Langs;
using Client.Utilities;
using Client.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views
{
    /// <summary>
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private bool _isLoaded = false;
        private bool _languageChanged = false;
        private bool _isGameActive = false;

        public Settings(bool changeHappened = false, bool isGameActive = false)
        {
            InitializeComponent();
            _languageChanged = changeHappened;
            _isGameActive |= isGameActive;
            LoadLanguages();
            ConfigureControls();
        }

        private void ConfigureControls()
        {
            if (_isGameActive)
            {
                ComboBoxLanguage.IsEnabled = false;
                ComboBoxLanguage.ToolTip = "Language cannot be changed during an active game.";
            }
        }

        private void LoadLanguages()
        {
            var languages = new List<LanguageOption>
            {
                new LanguageOption { DisplayCultureName = "English", CultureCode = "en-US" },
                new LanguageOption { DisplayCultureName = "Español", CultureCode = "es-MX" },
                new LanguageOption { DisplayCultureName = "Français", CultureCode = "fr" },
                new LanguageOption { DisplayCultureName = "Deutsch", CultureCode = "de" },
                new LanguageOption { DisplayCultureName = "Português", CultureCode = "pt" },
                new LanguageOption { DisplayCultureName = "日本語", CultureCode = "ja-JP" },
                new LanguageOption { DisplayCultureName = "中文", CultureCode = "zh-CN" },
                new LanguageOption { DisplayCultureName = "한국어", CultureCode = "ko-KR" }
            };

            ComboBoxLanguage.ItemsSource = languages;
            string currentLangCode = Lang.Culture.Name;
            var selectedLanguage = languages.FirstOrDefault(lang => lang.CultureCode == currentLangCode);

            if (selectedLanguage != null)
            {
                ComboBoxLanguage.SelectedItem = selectedLanguage;
            }
            else
            {
                ComboBoxLanguage.SelectedIndex = 0;
            }
            _isLoaded = true;
        }

        private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoaded) return;

            if (ComboBoxLanguage.SelectedItem is LanguageOption selectedOption)
            {
                string newLangCode = selectedOption.CultureCode;

                if (Lang.Culture.Name != newLangCode)
                {
                    Properties.Settings.Default.languageCode = newLangCode;
                    Properties.Settings.Default.Save();
                    Lang.Culture = new CultureInfo(newLangCode);

                    LocalizationHelper.ApplyLanguageFont();

                    _languageChanged = true;
                    RefreshWindow();
                }
            }
        }

        private void RefreshWindow()
        {
            var oldMode = Application.Current.ShutdownMode;
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            try
            {
                var newSettingsWindow = new Settings(true, _isGameActive);
                newSettingsWindow.Owner = this.Owner;
                newSettingsWindow.Show();

                if (Application.Current.MainWindow == this)
                {
                    Application.Current.MainWindow = newSettingsWindow;
                }

                this.Close();
            }
            finally 
            {
                Application.Current.ShutdownMode = oldMode;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Window windowToOpen;

            if (!string.IsNullOrEmpty(UserSession.SessionToken))
            {
                windowToOpen = new MainMenu();
            }
            else
            {
                windowToOpen = new TitleScreen();
            }

            if (_languageChanged)
            {
                NavigationHelper.NavigateTo(this, windowToOpen);
            }
            else
            {
                NavigationHelper.NavigateTo(this, this.Owner ?? windowToOpen);
            }
        }
    }
}