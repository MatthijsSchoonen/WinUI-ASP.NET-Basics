using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Models;
using WinUI_Basics.Controllers;
using System.Collections.ObjectModel;

namespace WinUI_Basics.Views
{
    public sealed partial class CreatePizzaPage : Page
    {
        MainWindow _mainWindow;
        public CreatePizzaPage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            LoadToppings();
        }

        private async void LoadToppings()
        {
            ObservableCollection<Toppings> toppings = await ToppingController.GetAllToppings();
            ToppingsListView.ItemsSource = toppings;
        }

        private async void CreatePizza_Click(object sender, RoutedEventArgs e)
        {
            var selectedToppings = ToppingsListView.SelectedItems.Cast<Toppings>().ToList();
            var pizza = new Pizza
            {
                Name = PizzaName.Text,
                Price = decimal.Parse(PizzaPrice.Text),
                ImgUrl = PizzaImgUrl.Text,
                PizzaToppings = selectedToppings
            };

            bool success = await PizzaController.CreatePizza(pizza);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to create pizza.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("Pizza created successfully!", "Success", this.XamlRoot);
        }
    }
}
