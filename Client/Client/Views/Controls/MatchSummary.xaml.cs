using Client.Core;
using Client.Properties.Langs;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            var textMessageBrush = (SolidColorBrush)Application.Current.FindResource("AccentForegroundColor");

            if(winnerName == "PlayGameMultiplayer_Label_Tie")
            {
                TextBlockWinner.Text = Lang.PlayGameMultiplayer_Label_Tie;
                LabelSubtitle.Visibility = Visibility.Collapsed;
            }
            else if (winnerName == UserSession.Username)
            {
                TextBlockWinner.Text = Lang.MatchSummary_Label_Win;
            }
            else if (winnerName == Lang.Singleplayer_Title_TimeOver)
            {
                TextBlockWinner.Text = winnerName;
                TextBlockWinner.Foreground = textMessageBrush;
            }
            else
            {
                TextBlockWinner.Text = $"{winnerName} {Lang.MatchSummary_Label_Lost}";
                TextBlockWinner.Foreground = textMessageBrush;
            }

            TextBlockScore.Text = scoreText;
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