using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Models;
using WinUI_Basics.Controllers;
using System.Collections.ObjectModel;
using System.Net.Http;
using Windows.Storage.Pickers;
using System.IO;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;
using WinRT.Interop;
using System.Threading.Tasks;

namespace WinUI_Basics.Views
{
    public sealed partial class EditPizzaPage : Page
    {
        MainWindow _mainWindow;
        Pizza _pizza;
        private string _uploadedImageUrl;

        public EditPizzaPage(MainWindow mainWindow, Pizza pizza)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            _pizza = pizza;
            InitializePage();
        }

        private async void InitializePage()
        {
            await LoadToppings();
            LoadPizzaDetails();
        }

        private async Task LoadToppings()
        {
            ObservableCollection<Toppings> toppings = await ToppingController.GetAllToppings();
            ToppingsListView.ItemsSource = toppings;
        }

        private void LoadPizzaDetails()
        {
            PizzaName.Text = _pizza.Name;
            PizzaPrice.Text = _pizza.Price.ToString();
            _uploadedImageUrl = _pizza.ImgUrl;

            // Clear previous selections
            ToppingsListView.SelectedItems.Clear();

            // Select the toppings
            foreach (var pizzaTopping in _pizza.PizzaToppings)
            {
                foreach (var topping in ToppingsListView.Items)
                {
                    if (topping is Toppings t && t.Id == pizzaTopping.Id)
                    {
                        ToppingsListView.SelectedItems.Add(t);
                        break;
                    }
                }
            }
        }

        private async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new FileOpenPicker();
            openFileDialog.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openFileDialog.FileTypeFilter.Add(".jpg");
            openFileDialog.FileTypeFilter.Add(".png");

            var hwnd = WindowNative.GetWindowHandle(_mainWindow);
            InitializeWithWindow.Initialize(openFileDialog, hwnd);

            Windows.Storage.StorageFile file = await openFileDialog.PickSingleFileAsync();
            if (file != null)
            {
                _uploadedImageUrl = await ImageController.UploadImage(file);
                if (string.IsNullOrEmpty(_uploadedImageUrl))
                {
                    MainWindow.ShowNotification("Failed to upload image.", "Error", this.XamlRoot);
                    return;
                }
                MainWindow.ShowNotification("Image uploaded successfully!", "Success", this.XamlRoot);
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            var selectedToppings = ToppingsListView.SelectedItems.Cast<Toppings>().ToList();
            if (selectedToppings.Count == 0)
            {
                MainWindow.ShowNotification("Please select at least one topping.", "Error", this.XamlRoot);
                return;
            }

            if (string.IsNullOrEmpty(PizzaName.Text))
            {
                MainWindow.ShowNotification("Please enter a pizza name.", "Error", this.XamlRoot);
                return;
            }

            if (!decimal.TryParse(PizzaPrice.Text, out decimal price))
            {
                MainWindow.ShowNotification("Please enter a valid price.", "Error", this.XamlRoot);
                return;
            }

            _pizza.Name = PizzaName.Text;
            _pizza.Price = price;
            _pizza.ImgUrl = _uploadedImageUrl;
            _pizza.PizzaToppings = selectedToppings;

            bool success = await PizzaController.EditPizza(_pizza);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to update pizza.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("Pizza updated successfully!", "Success", this.XamlRoot);
            _mainWindow.ToPizzaPage();
        }

        private async void DeletePizza_Click(object sender, RoutedEventArgs e)
        {
            bool success = await PizzaController.DeletePizza(_pizza.Id);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to delete pizza.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("Pizza deleted successfully!", "Success", this.XamlRoot);
            _mainWindow.ToPizzaPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToPizzaPage();
        }
    }
}
