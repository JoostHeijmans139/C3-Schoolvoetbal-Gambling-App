using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using GamblingApp.Data;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GamblingApp;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class GamblePage : Page
{
    public GamblePage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        gameListView.DataContext = App.GamesVM;
    }

    private void betButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var game = (Game)button.DataContext;

        if (BetStorage.bets.Any(bets => bets.Game.Id == game.Id))
        {
            errorInfoBar.IsOpen = true;
            errorInfoBar.Severity = InfoBarSeverity.Warning;
            errorInfoBar.Content = "Je hebt al ingezet op deze wedstrijd.";
            return;
        }

        var dialog = new BetDialog(game);
        dialog.XamlRoot = this.XamlRoot;

        var result = dialog.ShowAsync();
    }

    private void closeFlyout_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var flyout = FlyoutBase.GetAttachedFlyout(button);
        flyout.Hide();
    }
}
