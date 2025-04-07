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
    public sealed partial class UserPage : Page
    {
        MainWindow _mainWindow;
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public UserPage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            LoadUsers();
        }

        private async void LoadUsers()
        {
            var users = await AccountController.GetUsers();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is User user)
            {
                if(user.RoleId == 1)
                {
                    MainWindow.ShowNotification("You cannot edit the admin user.", "Error", this.XamlRoot);
                    return;
                }
                _mainWindow.ToEditUserPage(user);
            }
        }
    }
}
