using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using WinUI_Basics.Models;
using WinUI_Basics.Views;
using System.Collections.ObjectModel;
using Windows.Networking.NetworkOperators;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI_Basics
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public static User? _LoggedInUser;
        public static ObservableCollection<Models.Pizza> _Cart = new();
        public MainWindow()
        {
            this.InitializeComponent();
            Nav.BackRequested += OnBackRequested;
            Nav.ItemInvoked += OnNavigationViewItemInvoked; // Add this line
            ToLogin();
        }


        public void ToLogin()
        {
            Nav.IsPaneVisible = false;
            MainFrame.Content = new LoginPage(this);
        }

        public void ToRegister()
        {
            Nav.IsPaneVisible = false;
            MainFrame.Content = new RegisterPage(this);
        }

        public void ToMenuPage()
        {
            MainFrame.Content = new MenuPage(this);
            Nav.IsPaneVisible = true;
            UpdateNavigationView();
        }

        public void ToCartPage()
        {
            MainFrame.Content = new CartPage();
        }

        public void ToReceiptPage()
        {
            MainFrame.Content = new ReceiptPage();
        }

        public void ToOrderPage()
        {
            MainFrame.Content = new OrderPage();
        }

        private void UpdateNavigationView()
        {
            Nav.IsPaneVisible = true;

            foreach (var item in Nav.MenuItems)
            {
                if (item is NavigationViewItem navItem)
                {
                    navItem.Visibility = Visibility.Collapsed;
                }
                else if (item is NavigationViewItemHeader navHeader)
                {
                    navHeader.Visibility = Visibility.Collapsed;
                }
            }

            // Show common items for all logged-in users
            SetNavigationViewItemVisibility("MenuPage", Visibility.Visible);
            SetNavigationViewItemVisibility("CartPage", Visibility.Visible);
            SetNavigationViewItemVisibility("ReceiptsPage", Visibility.Visible);

            if (_LoggedInUser?.Role?.Name == "Employee" || _LoggedInUser?.Role?.Name == "Admin")
            {
                // Show employee items
                SetNavigationViewItemVisibility("EmployeeHeader", Visibility.Visible);
                SetNavigationViewItemVisibility("OrdersPage", Visibility.Visible);
                SetNavigationViewItemVisibility("PizzasPage", Visibility.Visible);
                SetNavigationViewItemVisibility("ToppingsPage", Visibility.Visible);
            }

            if (_LoggedInUser?.Role?.Name == "Admin")
            {
                // Show admin items
                SetNavigationViewItemVisibility("AdminHeader", Visibility.Visible);
                SetNavigationViewItemVisibility("UsersPage", Visibility.Visible);
            }
        }

        private void SetNavigationViewItemVisibility(string tag, Visibility visibility)
        {
            foreach (var item in Nav.MenuItems)
            {
                if (item is NavigationViewItem navItem && navItem.Tag?.ToString() == tag)
                {
                    navItem.Visibility = visibility;
                }
                else if (item is NavigationViewItemHeader navHeader && navHeader.Content?.ToString() == tag)
                {
                    navHeader.Visibility = visibility;
                }
            }
        }



        private void OnNavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem selectedItem)
            {
                switch (selectedItem.Tag)
                {
                    case "MenuPage":
                        ToMenuPage();
                        break;
                    case "CartPage":
                        ToCartPage();
                        break;
                    case "ReceiptsPage":
                        ToReceiptPage();
                        break;
                    case "OrdersPage":
                        ToOrderPage();
                        break;
                    case "PizzasPage":
                        //to pizzas page
                        break;
                    case "ToppingsPage":
                        //to toppings page
                        break;
                    case "UsersPage":
                        //to users page
                        break;
                    case "ProfilePage":
                        //to profile page
                        break;
                    case "Logout":
                        _LoggedInUser = null;
                        ToLogin();
                        break;
                }
            }
        }


        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }


        public static async void ShowNotification(string message, string title, XamlRoot xamlRoot)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "Close",
                XamlRoot = xamlRoot
            };

            var result = await dialog.ShowAsync();
        }

    }
}
