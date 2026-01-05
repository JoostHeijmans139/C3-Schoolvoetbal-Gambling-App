using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using GamblingApp.Data;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.Storage;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Protection.PlayReady;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GamblingApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            rootGrid.DataContext = App.PointsVM;

            EvaluateBets();

            this.Closed += MainWindow_Closed;
            LoadPoints();
        }

        public async void EvaluateBets()
        {
            await Task.WhenAll(GetGames(), BetStorage.LoadBets());

            var pendingBets = BetStorage.bets.Where(bet => bet.Status == BettingStatus.Pending);

            foreach (var bet in pendingBets)
            {
                var game = App.GamesVM.Games.Where(game => game.Id == bet.Game.Id).First();
                if (game.ScoreTeam1 != null && game.ScoreTeam2 != null)
                {
                    if (game.ScoreTeam1 == game.ScoreTeam2)
                    {
                        bet.Status = BettingStatus.Draw;
                        App.PointsVM.Points += bet.Amount;
                        continue;
                    }


                    bool isteam1winner = game.ScoreTeam1 > game.ScoreTeam2;
                    bool isBetOnTeam1 = game.Team1.Id == bet.Team.Id;
                    bool isBetCorrect = isteam1winner == isBetOnTeam1;

                    if (isBetCorrect)
                    {
                        bet.Status = BettingStatus.Won;
                        App.PointsVM.Points += bet.Amount * 2;
                    }
                    else
                    {
                        bet.Status = BettingStatus.Lost;
                    }
                }
            }
        }

        public async Task GetGames()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://schoolvoetbal.test/api/wedstrijden");

                if (!response.IsSuccessStatusCode)
                {
                    messageInfoBar.Severity = InfoBarSeverity.Error;
                    messageInfoBar.Content = "Kon geen verbinding maken met de server:\n" + await response.Content.ReadAsStringAsync();
                    messageInfoBar.IsOpen = true;
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var games = JsonSerializer.Deserialize<GameResponse>(body).Games;
                    App.GamesVM.Games = new System.Collections.ObjectModel.ObservableCollection<Game>(games);
                }
            }
            catch (HttpRequestException exception)
            {
                messageInfoBar.Severity = InfoBarSeverity.Error;
                messageInfoBar.Content = "Kon geen verbinding maken met de server:\n" + exception.Message;
                messageInfoBar.IsOpen = true;
            }
            catch (JsonException exception)
            {
                messageInfoBar.Severity = InfoBarSeverity.Error;
                messageInfoBar.Content = "Fout bij verwerken van de wedstrijden:\n" + exception.Message;
                messageInfoBar.IsOpen = true;
            }
            loadingProgressRing.IsActive = false;
            contentFrame.Opacity = 1;
            contentFrame.IsHitTestVisible = true;
            CheckBets();
        }

        public void CheckBets()
        {
            var games = App.GamesVM.Games;
            var bets = BetStorage.bets;
        }

        private void navigationSelectorBar_SelectionChanged(SelectorBar sender, SelectorBarSelectionChangedEventArgs args)
        {
            var selectedItem = sender.SelectedItem;
            var selectedIndex = sender.Items.IndexOf(selectedItem);

            switch (selectedIndex)
            {
                case 0:
                    contentFrame.Navigate(typeof(HomePage));
                    break;
                case 1:
                    contentFrame.Navigate(typeof(GamblePage));
                    break;
                default:
                    break;
            }
        }

        public void SavePoints()
        {
            var localSettings = ApplicationData.GetDefault().LocalSettings;
            localSettings.Values["points"] = App.PointsVM.Points;
        }

        public void LoadPoints()
        {
            var localSettings = ApplicationData.GetDefault().LocalSettings;

            if (!localSettings.Values.TryGetValue("points", out var points))
            {
                App.PointsVM.Points = 10;
            }
            else App.PointsVM.Points = (int)points;
        }

        public async void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            SavePoints();
            await BetStorage.SaveBets();
        }

        private void pointsButton_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(BetHistoryPage));
            navigationSelectorBar.SelectedItem = null;
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            loadingProgressRing.IsActive = true;
            contentFrame.Opacity = 0.4;
            contentFrame.IsHitTestVisible = false;

            EvaluateBets();
        }
    }
}
