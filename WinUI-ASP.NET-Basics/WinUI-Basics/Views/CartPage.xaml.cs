using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using WinUI_Basics.Controllers;

namespace WinUI_Basics.Views
{
    public sealed partial class CartPage : Page
    {
        public ObservableCollection<Models.Pizza> Pizzas { get; set; } = new ObservableCollection<Models.Pizza>(); // Change Views.Pizza to Models.Pizza

        public CartPage()
        {
            this.InitializeComponent();

            Pizzas.CollectionChanged += (s, e) => UpdateTotalPrice();
            LoadPizzas();
        }

        private void LoadPizzas()
        {
            // Sample data
            Pizzas = MainWindow._Cart;
            PizzaListView.ItemsSource = Pizzas;
            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            double total = 0;
            foreach (var pizza in Pizzas)
            {
                total += (double)pizza.Price; // Cast to double
            }
            TotalPriceTextBlock.Text = $"${total:F2}";
        }

        private void RemovePizza_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Pizza pizza)
            {
                Pizzas.Remove(pizza);
                UpdateTotalPrice();
            }
        }

        private async void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            bool succes = await OrderController.CreateOrder(Pizzas);
            if (succes)
            {
                Pizzas.Clear();
                UpdateTotalPrice();
                MainWindow._Cart.Clear();
                MainWindow.ShowNotification("Order Created", "Your order has been created successfully.", this.XamlRoot);
            }
            else
            {
                MainWindow.ShowNotification("Order Failed", "Failed to create your order. Please try again.", this.XamlRoot);
            }
        }
    }

}
