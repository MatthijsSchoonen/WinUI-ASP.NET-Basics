using System;
using System.Collections.Generic;
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
using WinUI_Basics.Controllers;
using WinUI_Basics.Helpers;
using WinUI_Basics.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI_Basics.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditProfilePage : Page
    {
        MainWindow _mainWindow;
        public EditProfilePage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            if (MainWindow._LoggedInUser != null)
            {
                NameTextBox.Text = MainWindow._LoggedInUser.Name;
                EmailTextBox.Text = MainWindow._LoggedInUser.Email;
            }
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow._LoggedInUser != null)
            {
                MainWindow._LoggedInUser.Name = NameTextBox.Text;
                MainWindow._LoggedInUser.Email = EmailTextBox.Text;

                bool success = await AccountController.EditUser(MainWindow._LoggedInUser);
                if (success)
                {
                    _mainWindow.ToProfilePage();
                }
                else
                {
                    ProfileErrorMessage.Text = "Failed to update profile.";
                }
            }
        }

        private async void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow._LoggedInUser != null)
            {
                if (string.IsNullOrEmpty(CurrentPasswordBox.Password) || string.IsNullOrEmpty(NewPasswordBox.Password) || string.IsNullOrEmpty(ConfirmNewPasswordBox.Password))
                {
                    PasswordErrorMessage.Text = "Please fill in all fields.";
                    return;
                }

                if (NewPasswordBox.Password != ConfirmNewPasswordBox.Password)
                {
                    PasswordErrorMessage.Text = "New passwords do not match.";
                    return;
                }
                if (!ValidateHelper.IsPasswordValid(NewPasswordBox.Password))
                {
                    PasswordErrorMessage.Text = "New password must be at least 8 characters long and contain a symbol, a number, and both upper and lower case letters.";
                    return;
                }
                bool passwordCorrect = await AccountController.CheckUserCredentials(MainWindow._LoggedInUser.Email, CurrentPasswordBox.Password);

                if (!passwordCorrect)
                {
                    PasswordErrorMessage.Text = "Current password is incorrect.";
                    return;
                }
                bool success = await AccountController.UpdatePassword(MainWindow._LoggedInUser.Id, NewPasswordBox.Password);
                if (success)
                {
                    PasswordErrorMessage.Text = "Password updated successfully.";
                }
                else
                {
                    PasswordErrorMessage.Text = "Failed to update password.";
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.ToProfilePage();
        }
    }
}
