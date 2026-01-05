using Client.Helpers;
using Client.Properties.Langs;
using System;
using System.Collections.Generic;
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

namespace Client.Views.Controls
{
    /// <summary>
    /// Interaction logic for MatchSummary.xaml
    /// </summary>
    public partial class MatchSummary : Window
    {
        public MatchSummary(string winnerName, string scoreText)
        {
            InitializeComponent();
            SolidColorBrush textMessageBrush;
            string lightTextColor = "AccentForegroundColor";
            textMessageBrush = (SolidColorBrush)Application.Current.FindResource(lightTextColor);

            if (winnerName == UserSession.Username)
            {
                TextBlockWinner.Text = Lang.MatchSummary_Label_Win;
            }
            else if(winnerName == Lang.Singleplayer_Title_TimeOver)
            {
                TextBlockWinner.Text = winnerName;
                TextBlockWinner.Foreground = textMessageBrush;
            }
            else
            {
                TextBlockWinner.Text = $"{winnerName}  {Lang.MatchSummary_Label_Lost}";
                TextBlockWinner.Foreground = textMessageBrush;
            }
                
            TextBlockScore.Text =scoreText;
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

         private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}