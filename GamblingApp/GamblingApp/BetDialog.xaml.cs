using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GamblingApp.Data;
using Microsoft.UI.Xaml.Controls;

namespace GamblingApp
{
    sealed partial class BetDialog : ContentDialog
    {
        Game game;
        public BetDialog(Game game)
        {
            this.InitializeComponent();

            this.game = game;

            Team[] teams = { game.Team1, game.Team2 };
            teamComboBox.ItemsSource = teams;
            teamComboBox.SelectedIndex = 0;

            tournamentTitleTextBlock.Text = game.Tournament.Name;
            gameDescriptionTextBlock.Text = $"Wedstrijd {game.FormattedStart}, {game.Team1.Name} tegen {game.Team2.Name}.";
        }

        private void betButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var team = teamComboBox.SelectedItem as Team;
            var amount = (int)amountNumberBox.Value;

            if (team == null) return;
            if (amount < 0) return;
            if (amount > App.PointsVM.Points) return;

            if (BetStorage.bets.Any(bet => bet.Game.Id == game.Id))
            { 
                return;
            }

            var bet = new Bet()
            {
                Status = BettingStatus.Pending,
                Amount = amount,
                Game = game,
                Team = team,
                PlacedAt = DateTime.Now,
            };

            App.PointsVM.Points -= bet.Amount;
            BetStorage.AddBet(bet);
            this.Hide();
        }

        private void cancelButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            this.Hide();
        }

        private void teamComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSummary();
        }

        private void amountNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            var value = Math.Floor(amountNumberBox.Value);
            amountNumberBox.Value = value;

            UpdateSummary();
        }
            
        private void UpdateSummary()
        {
            var team = teamComboBox.SelectedItem as Team;
            if (team == null) return;
            var points = amountNumberBox.Value;
            string sentence = $"Wanneer {team.Name} wint, verdien je {points*2} punten.\nBij gelijkspel krijg je je punten terug.";
            summaryTextBlock.Text = sentence;
        }
    }
}
