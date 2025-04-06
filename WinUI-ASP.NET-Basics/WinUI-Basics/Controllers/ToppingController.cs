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
    class ToppingController
    {
        public static async Task<ObservableCollection<Toppings>> GetAllToppings()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7114/api/Topping/GetAll");
            request.Headers.Add("accept", "*/*");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            var toppings = JsonSerializer.Deserialize<ObservableCollection<Toppings>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return toppings ?? new ObservableCollection<Toppings>();
        }
    }
}
