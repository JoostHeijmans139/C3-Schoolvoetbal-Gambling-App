using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using GamblingApp.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GamblingApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            GetGames();
        }

        public async void GetGames()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://schoolvoetbal.test/api/wedstrijden");

                if (!response.IsSuccessStatusCode)
                {
                    messageInfoBar.Severity = InfoBarSeverity.Error;
                    messageInfoBar.Content = "Kon geen verbinding maken met de server:\n" + response.Content.ReadAsStringAsync();
                    messageInfoBar.IsOpen = true;
                    return;
                }

                var body = await response.Content.ReadAsStringAsync();
                Game[] games = JsonSerializer.Deserialize<GameResponse>(body).Data;

                gameListView.ItemsSource = games;
                //gameListView.ItemsSource = games.Where(g => g.ScoreTeam1 != null);
            }
            catch (HttpRequestException exception)
            {
                messageInfoBar.Severity = InfoBarSeverity.Error;
                messageInfoBar.Content = "Kon geen verbinding maken met de server:\n" + exception.Message;
                messageInfoBar.IsOpen = true;
            }
            loadingProgressRing.IsActive = false;
        }
    }
}
