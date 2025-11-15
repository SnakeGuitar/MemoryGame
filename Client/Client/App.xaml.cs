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
        protected override void OnStartup(StartupEventArgs e)
        {
            string savedLangCode = Client.Properties.Settings.Default.languageCode;

            if (string.IsNullOrEmpty(savedLangCode))
            {
                savedLangCode = "en-US"; // O "en-US"
            }

            Client.Properties.Langs.Lang.Culture = new CultureInfo(savedLangCode);

            base.OnStartup(e);
        }
    }
}
