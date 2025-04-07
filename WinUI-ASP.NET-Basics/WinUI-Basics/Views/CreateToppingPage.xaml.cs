using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Controllers;
using WinUI_Basics.Models;

namespace WinUI_Basics.Views
{
    public sealed partial class CreateToppingPage : Page
    {
        MainWindow _mainWindow;

        public CreateToppingPage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
        }

        private async void CreateTopping_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ToppingName.Text))
            {
                MainWindow.ShowNotification("Please enter a topping name.", "Error", this.XamlRoot);
                return;
            }

            var topping = new Toppings
            {
                Name = ToppingName.Text
            };

            bool success = await ToppingController.CreateTopping(topping);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to create topping.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("Topping created successfully!", "Success", this.XamlRoot);
            _mainWindow.ToToppingPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToToppingPage();
        }
    }
}
