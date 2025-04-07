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

namespace WinUI_Basics.Views
{
    public sealed partial class CreatePizzaPage : Page
    {
        MainWindow _mainWindow;
        private string _uploadedImageUrl;

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

        private async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new FileOpenPicker();
            openFileDialog.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openFileDialog.FileTypeFilter.Add(".jpg");
            openFileDialog.FileTypeFilter.Add(".png");

            // Get the window handle and set it for the picker
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

        private async void CreatePizza_Click(object sender, RoutedEventArgs e)
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

            var pizza = new Pizza
            {
                Name = PizzaName.Text,
                Price = price,
                ImgUrl = _uploadedImageUrl,
                PizzaToppings = selectedToppings
            };

            bool success = await PizzaController.CreatePizza(pizza);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to create pizza.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("Pizza created successfully!", "Success", this.XamlRoot);
            _mainWindow.ToPizzaPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToPizzaPage();
        }

    }

    public class UploadImageResponse
    {
        public string Url { get; set; }
    }
}
