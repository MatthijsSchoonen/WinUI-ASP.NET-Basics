using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using WinUI_Basics.Models;
using WinUI_Basics.Controllers;

namespace WinUI_Basics.Views
{
    public sealed partial class ReceiptPage : Page
    {
        ObservableCollection<Order> _receipts = new();
        public ReceiptPage()
        {
            this.InitializeComponent();
            LoadReceipt();
        }

        public async Task LoadReceipt()
        {
            var allOrders = await OrderController.GetAllOrders();
            var userOrders = allOrders.Where(o => o.UserId == MainWindow._LoggedInUser.Id).ToList();
            _receipts = new ObservableCollection<Order>(userOrders);
            ReceiptListView.ItemsSource = _receipts;
        }
    }
}
