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
using WinUI_Basics.Models;
using WinUI_Basics.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI_Basics.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        MainWindow _mainWindow;
        public RegisterPage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void Login_Click(Microsoft.UI.Xaml.Documents.Hyperlink sender, Microsoft.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            _mainWindow.ToLogin();
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Name.Text) || string.IsNullOrEmpty(Email.Text) || string.IsNullOrEmpty(Password.Password) || string.IsNullOrEmpty(PasswordConfirm.Password))
            {
                ErrorMessage.Text = "Please fill in all fields.";
                return;
            }
            if (!ValidateHelper.IsValidEmail(Email.Text))
            {
                ErrorMessage.Text = "Invalid email format.";
                return;
            }
            if (!ValidateHelper.IsPasswordValid(Password.Password))
            {
                ErrorMessage.Text = "Password must be at least 8 characters long and contain a symbol, a number, and both upper and lower case letters.";
                return;
            }
            if (Password.Password != PasswordConfirm.Password)
            {
                ErrorMessage.Text = "Passwords do not match.";
                return;
            }
          
           
            User user = new User
            {
                Name = Name.Text,
                Email = Email.Text,
                Password = Password.Password
            };

            bool success = await AccountController.RegisterUser(user);
            if (success)
            {
                _mainWindow.ToLogin();
            }
            else
            {
                ErrorMessage.Text = "Invalid email or password.";
            }
        }
    }
}
