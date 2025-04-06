using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Controllers;
using WinUI_Basics.Models;

namespace WinUI_Basics.Views
{
    public sealed partial class MenuPage : Page
    {
        MainWindow _mainWindow;
        ObservableCollection<Pizza> _Pizza = new ObservableCollection<Pizza>();
        ObservableCollection<Toppings> _Toppings = new ObservableCollection<Toppings>();
        ObservableCollection<Pizza> _FilteredPizzas = new ObservableCollection<Pizza>();

        public MenuPage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            LoadPizzas();
            LoadToppings();
        }

        public ObservableCollection<Pizza> FilteredPizzas => _FilteredPizzas;

        private async void LoadPizzas()
        {
            var pizzas = await PizzaController.GetPizzas();
            foreach (var pizza in pizzas)
            {
                _Pizza.Add(pizza);
                _FilteredPizzas.Add(pizza);
            }
        }

        private async void LoadToppings()
        {
            var toppings = await ToppingController.GetAllToppings();
            foreach (var topping in toppings)
            {
                _Toppings.Add(topping);
            }
            ToppingsFilter.ItemsSource = _Toppings;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterPizzas();
        }

        private void ToppingsFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterPizzas();
        }

        private void FilterPizzas()
        {
            var searchText = SearchBox.Text.ToLower();
            var selectedTopping = ToppingsFilter.SelectedItem as Toppings;

            _FilteredPizzas.Clear();

            foreach (var pizza in _Pizza)
            {
                if (pizza.Name.ToLower().Contains(searchText) &&
                    (selectedTopping == null || pizza.PizzaToppings.Any(t => t.Name == selectedTopping.Name)))
                {
                    _FilteredPizzas.Add(pizza);
                }
            }
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pizza = button?.Tag as Pizza;
            if (pizza != null)
            {
                MainWindow._Cart.Add(pizza);
            }
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = string.Empty;
            ToppingsFilter.SelectedItem = null;
            FilterPizzas();
        }
    }
}
