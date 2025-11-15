using Client.Properties.Langs;
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

        public Settings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string activeLanguage = Lang.Culture.Name;

            if (activeLanguage == "en-US")
            {
                ComboBoxLanguage.SelectedIndex = 1;
            }
            else
            {
                ComboBoxLanguage.SelectedIndex = 0;
            }

            isLoaded = true;
        }

        private void ComboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!isLoaded) return;

            string newLangCode = (ComboBoxLanguage.SelectedIndex == 0) ? "es-MX" : "en-US";

            if (Lang.Culture.Name != newLangCode)
            {
                Properties.Settings.Default.languageCode = newLangCode;
                Properties.Settings.Default.Save();
                Lang.Culture = new CultureInfo(newLangCode);

                languageChanged = true;

                RefreshWindow();
            }

            Properties.Settings.Default.languageCode = newLangCode;
            Properties.Settings.Default.Save();

            Properties.Langs.Lang.Culture = new CultureInfo(newLangCode);

            Window owner = this.Owner;
            this.Close();

            var newSettingsWindow = new Settings();
            newSettingsWindow.Owner = owner;
            newSettingsWindow.WindowState = this.WindowState;
            newSettingsWindow.Show();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {

            Window mainMenu = this.Owner;

            if (languageChanged)
            {
                mainMenu.Close();

                var newMainMenu = new MainMenu();
                newMainMenu.WindowState = this.WindowState;
                newMainMenu.Show();
            }
            else
            {
                if (mainMenu != null)
                {
                    mainMenu.WindowState = this.WindowState;
                    mainMenu.Show();
                }
            }

            this.Close();
        }

        private void RefreshWindow()
        {
            Window owner = this.Owner;
            this.Close();

            var newSettingsWindow = new Settings();
            newSettingsWindow.Owner = owner;
            newSettingsWindow.WindowState = this.WindowState;
            newSettingsWindow.Show();
        }
    }

}

