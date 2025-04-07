using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinUI_Basics.Models;

namespace WinUI_Basics.Controllers
{
    class OrderController
    {
        public static async Task<bool> CreateOrder(ObservableCollection<Pizza> pizzas)
        {
            try
            {
                User? user = MainWindow._LoggedInUser;
                if (user == null || pizzas.Count == 0)
                {
                    return false;
                }

                // Create the order object
                Order order = new Order
                {
                    UserId = user.Id,
                    User = user,
                    StatusId = 1,
                    OrderedAt = DateTime.Now,
                    Pizzas = pizzas.ToList()
                };

                // Serialize the order object to JSON
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                string jsonOrder = JsonSerializer.Serialize(order, options);

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7114/api/Order/CreateOrder");
                request.Headers.Add("accept", "*/*");
                var content = new StringContent(jsonOrder, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
