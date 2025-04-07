using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Controllers;
using WinUI_Basics.Models;

namespace WinUI_Basics.Views
{
    public sealed partial class EditToppingPage : Page
    {
        MainWindow _mainWindow;
        Toppings _topping;

        public EditToppingPage(MainWindow mainWindow, Toppings topping)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            _topping = topping;
            LoadToppingDetails();
        }

        private void LoadToppingDetails()
        {
            ToppingName.Text = _topping.Name;
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ToppingName.Text))
            {
                MainWindow.ShowNotification("Please enter a topping name.", "Error", this.XamlRoot);
                return;
            }

            _topping.Name = ToppingName.Text;

            bool success = await ToppingController.EditTopping(_topping);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to update topping.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("Topping updated successfully!", "Success", this.XamlRoot);
            _mainWindow.ToToppingPage();
        }

        private async void DeleteTopping_Click(object sender, RoutedEventArgs e)
        {
            bool success = await ToppingController.DeleteTopping(_topping.Id);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to delete topping.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("Topping deleted successfully!", "Success", this.XamlRoot);
            _mainWindow.ToToppingPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToToppingPage();
        }
    }
}
