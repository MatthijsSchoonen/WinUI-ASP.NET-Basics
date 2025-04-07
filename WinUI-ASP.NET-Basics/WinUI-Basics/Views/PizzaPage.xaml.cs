using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Models;
using WinUI_Basics.Controllers;

namespace WinUI_Basics.Views
{
    public sealed partial class PizzaPage : Page
    {
        MainWindow _mainWindow;
        ObservableCollection<Pizza> _Pizzas = new ObservableCollection<Pizza>();

        public PizzaPage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            LoadPizzas();
        }

        public ObservableCollection<Pizza> Pizzas => _Pizzas;

        private async void LoadPizzas()
        {
            var pizzas = await PizzaController.GetPizzas();
            foreach (var pizza in pizzas)
            {
                _Pizzas.Add(pizza);
            }
        }

        private void AddPizzaButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToCreatePizzaPage();
        }

        private void EditPizzaButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pizza = button?.Tag as Pizza;
            if (pizza != null)
            {
                _mainWindow.ToEditPizzaPage(pizza);
            }
        }
    }
}
