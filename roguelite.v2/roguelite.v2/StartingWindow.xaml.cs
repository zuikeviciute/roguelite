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
using System.Text.RegularExpressions;

namespace roguelite
{
    /// <summary>
    /// Interaction logic for StartingWindow.xaml
    /// </summary>
    public partial class StartingWindow : Window
    {
        public StartingWindow()
        {
            InitializeComponent();
            ControlsOverlay.Visibility = Visibility.Hidden;
            CreditOverlay.Visibility = Visibility.Hidden;
            ScoreboardOverlay.Visibility = Visibility.Hidden;

            string[] scoreboard = System.IO.File.ReadAllLines(@"D:\study notes\C#\roguelite-v3\Scoreboard.txt");
            foreach (string str in scoreboard)
            {
                scoreboardtxt.AppendText(str + "\n");
            }
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Owner.Show();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Owner.Close();
        }

        private void controls_Click(object sender, RoutedEventArgs e)
        {
            ControlsOverlay.Visibility = Visibility.Visible;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            ControlsOverlay.Visibility = Visibility.Hidden;
        }

        private void credits_Click(object sender, RoutedEventArgs e)
        {
            CreditOverlay.Visibility = Visibility.Visible;
        }

        private void closeCredits_Click(object sender, RoutedEventArgs e)
        {
            CreditOverlay.Visibility = Visibility.Hidden;
        }

        private void roguelite_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner.Close();
        }

        private void Scoreboard_Click(object sender, RoutedEventArgs e)
        {
            ScoreboardOverlay.Visibility = Visibility.Visible;
        }

        private void close1_Click(object sender, RoutedEventArgs e)
        {
            ScoreboardOverlay.Visibility = Visibility.Hidden;
        }
    }
}
