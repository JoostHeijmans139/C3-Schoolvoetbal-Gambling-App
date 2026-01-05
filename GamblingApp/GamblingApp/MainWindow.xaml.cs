using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
            contentFrame.Navigate(typeof(HomePage));
            rootGrid.DataContext = App.PointsVM;

            EvaluateBets();

            this.Closed += MainWindow_Closed;
            LoadPoints();
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
                    contentFrame.Navigate(typeof(HomePage));
                    break;
                case 2:
                    contentFrame.Navigate(typeof(HomePage));
                    break;
                case 3:
                    contentFrame.Navigate(typeof(HomePage));
                    break;
                default:
                    contentFrame.Navigate(typeof(HomePage));
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
            var vm = (PointsViewModel)rootGrid.DataContext;
            var localSettings = ApplicationData.GetDefault().LocalSettings;

            if (!localSettings.Values.TryGetValue("points", out var points))
            {
                App.PointsVM.Points = 10;
            }
            else App.PointsVM.Points = (int)points;
        }

        public void MainWindow_Closed(object sender, WindowEventArgs e)
        {
            SavePoints();
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
