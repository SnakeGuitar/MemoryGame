using Client.Properties.Langs;
using Client.Utilities;
using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Views
{
    /// <summary>
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private bool isLoaded = false;
        private bool languageChanged = false;

        public Settings(bool changeHappened = false)
        {
            InitializeComponent();
            languageChanged = changeHappened;
            LoadLanguages();
        }

        private void LoadLanguages()
        {
            var languages = new List<LanguageOption>
            {
                new LanguageOption { DisplayCultureName = "Español", CultureCode = "es-MX" },
                new LanguageOption { DisplayCultureName = "English", CultureCode = "en-US" },
                new LanguageOption { DisplayCultureName = "日本語", CultureCode = "ja-JP" }
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

            isLoaded = true;

        }

        private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded)
            {
                return;
            }

            LanguageOption selectedOption = ComboBoxLanguage.SelectedItem as LanguageOption;

            if (selectedOption == null)
            {
                return;
            }

            string newLangCode = selectedOption.CultureCode;

            if (Lang.Culture.Name != newLangCode)
            {
                Properties.Settings.Default.languageCode = newLangCode;
                Properties.Settings.Default.Save();
                Lang.Culture = new CultureInfo(newLangCode);

                languageChanged = true;

                RefreshWindow();
            }
        }

        private void RefreshWindow()
        {
            Window owner = this.Owner;
            this.Close();

            var newSettingsWindow = new Settings(true);
            newSettingsWindow.Owner = owner;
            newSettingsWindow.WindowState = this.WindowState;
            newSettingsWindow.Show();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (languageChanged)
            {
                var oldMainMenu = this.Owner;

                var newMainMenu = new MainMenu();
                newMainMenu.WindowState = this.WindowState;
                newMainMenu.Show();

                this.Close();

                if (oldMainMenu != null)
                {
                    oldMainMenu.Close();
                }
            }
            else
            {
                if (this.Owner != null)
                {
                    this.Owner.WindowState = this.WindowState;
                    this.Owner.Show();
                }
                else
                {
                    var mainMenu = new MainMenu();
                    mainMenu.WindowState = this.WindowState;
                    mainMenu.Show();
                }
                    this.Close();
            }

        }
    }
}