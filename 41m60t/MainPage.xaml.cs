using _41m60t.Assets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
namespace _41m60t
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.Content = "\uE72C";
            MainFrame.Navigate(typeof(TrainingPage), false);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (black.IsChecked == true)
            {
                App.main_color = Colors.Black; 
            }
            else if (orange.IsChecked == true)
            {
                App.main_color = Colors.Orange;
            }
            else if (green.IsChecked == true)
            {
                App.main_color = Colors.Green;
            }
            else if (blue.IsChecked == true)
            {
                App.main_color = Colors.Blue;
            }
        }

        private async void StackPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MessageDialog a = new MessageDialog("Copyrights © 2017, Elyar Adil.\nAll rights reserved.");
            await a.ShowAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(typeof(TrainingPage), true);
        }
    }
}
