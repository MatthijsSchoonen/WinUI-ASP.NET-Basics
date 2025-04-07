using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI_Basics.Controllers;
using WinUI_Basics.Models;

namespace WinUI_Basics.Views
{
    public sealed partial class OrderPage : Page
    {
        ObservableCollection<Order> _Orders = new ObservableCollection<Order>();
        ObservableCollection<Status> _Statuses = new ObservableCollection<Status>();
        ObservableCollection<Order> _FilteredOrders = new ObservableCollection<Order>();

        public OrderPage()
        {
            this.InitializeComponent();
            LoadOrders();
            LoadStatuses();
        }

        public ObservableCollection<Order> FilteredOrders => _FilteredOrders;

        private async void LoadOrders()
        {
            _FilteredOrders.Clear();
            var orders = await OrderController.GetAllOrders();
            foreach (var order in orders)
            {
                _Orders.Add(order);
                _FilteredOrders.Add(order);
            }
        }

        private async void LoadStatuses()
        {
            var statuses = await StatusController.GetAllStatus();
            foreach (var status in statuses)
            {
                _Statuses.Add(status);
            }
            StatusFilter.ItemsSource = _Statuses;
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterOrders();
        }

        private void FilterOrders()
        {
            var selectedStatus = StatusFilter.SelectedItem as Status;

            _FilteredOrders.Clear();

            foreach (var order in _Orders)
            {
                if (selectedStatus == null || order.StatusId == selectedStatus.Id)
                {
                    _FilteredOrders.Add(order);
                }
            }
        }

        private async void UpdateStatusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button?.Tag as Order;
            if (order != null)
            {
                // Update the status logic here
                var newStatusId = order.StatusId + 1; // Example logic to increment status
                var success = await OrderController.UpdateStatus(order.Id, newStatusId);
                if (success)
                {
                    order.StatusId = newStatusId;
                    FilterOrders();
                    LoadOrders();
                }
            }
        }

        private void ResetFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            StatusFilter.SelectedItem = null;
            FilterOrders();
        }
    }
}
