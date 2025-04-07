using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Models;
using WinUI_Basics.Controllers;

namespace WinUI_Basics.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ToppingPage : Page
    {
        MainWindow _mainWindow;
        public ObservableCollection<Toppings> Toppings { get; set; } = new ObservableCollection<Toppings>();

        public ToppingPage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            LoadToppings();
        }

        private async void LoadToppings()
        {
            var toppings = await ToppingController.GetAllToppings();
            foreach (var topping in toppings)
            {
                Toppings.Add(topping);
            }
        }

        private void AddToppingButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToCreateToppingPage();
        }

        private void EditToppingButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Toppings topping)
            {
                _mainWindow.ToEditToppingPage(topping);
            }
        }
    }
}
