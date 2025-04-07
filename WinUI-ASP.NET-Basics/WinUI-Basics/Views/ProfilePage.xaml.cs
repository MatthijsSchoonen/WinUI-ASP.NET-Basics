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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI_Basics.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        MainWindow _mainWindow;
        public ProfilePage(MainWindow mainWindow)
        {
            this.InitializeComponent();
            _mainWindow = mainWindow;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            if (MainWindow._LoggedInUser != null)
            {
                NameTextBlock.Text = MainWindow._LoggedInUser.Name;
                EmailTextBlock.Text = MainWindow._LoggedInUser.Email;
                RoleTextBlock.Text = MainWindow._LoggedInUser.Role?.Name ?? "N/A";
            }
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow._LoggedInUser != null)
            {
                _mainWindow.ToEditProfilePage();
            }
        }
    }
}
