using System;
using System.Diagnostics;
using System.Windows;

namespace Client.Views.Controls
{
    public partial class InviteFriendDialog : Window
    {
        private readonly string _lobbyCode;

        public InviteFriendDialog(string lobbyCode)
        {
            InitializeComponent();
            _lobbyCode = lobbyCode;
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Por favor ingresa un correo.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SendEmail(email);
            this.Close();
        }

        private void SendEmail(string targetEmail)
        {
            try
            {
                string subject = "¡Únete a mi partida de Memory Game!";
                string body = $"Hola, te invito a jugar.\n\nEl código del Lobby es: {_lobbyCode}\n\n¡Entra rápido!";

                string mailtoUri = $"mailto:{targetEmail}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

                Process.Start(new ProcessStartInfo(mailtoUri) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir la aplicación de correo: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}