using System;
using System.Collections.ObjectModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Models;
using WinUI_Basics.Controllers;
using System.Linq;

namespace WinUI_Basics.Views
{
    public sealed partial class EditUserPage : Page
    {
        MainWindow _mainWindow;
        User _user;
        public ObservableCollection<Role> Roles { get; set; } = new ObservableCollection<Role>();

        public EditUserPage(MainWindow mainWindow, User user)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            _user = user;
            LoadUserDetails();
            LoadRoles();
        }

        private void LoadUserDetails()
        {
            UserName.Text = _user.Name;
            UserEmail.Text = _user.Email;
        }

        private async void LoadRoles()
        {
            var roles = await RoleController.GetRoles();
            foreach (var role in roles)
            {
                Roles.Add(role);
            }
            UserRole.ItemsSource = Roles;
            UserRole.SelectedItem = Roles.FirstOrDefault(r => r.Id == _user.RoleId);
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UserName.Text) || string.IsNullOrEmpty(UserEmail.Text) || UserRole.SelectedItem == null)
            {
                MainWindow.ShowNotification("Please fill in all fields.", "Error", this.XamlRoot);
                return;
            }

            _user.Name = UserName.Text;
            _user.Email = UserEmail.Text;
            _user.RoleId = ((Role)UserRole.SelectedItem).Id;

            bool success = await AccountController.EditUser(_user);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to update user.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("User updated successfully!", "Success", this.XamlRoot);
            _mainWindow.ToUserPage();
        }

        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            bool success = await AccountController.DeleteUser(_user.Id);
            if (!success)
            {
                MainWindow.ShowNotification("Failed to delete user.", "Error", this.XamlRoot);
                return;
            }

            MainWindow.ShowNotification("User deleted successfully!", "Success", this.XamlRoot);
            _mainWindow.ToUserPage();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToUserPage();
        }
    }
}
