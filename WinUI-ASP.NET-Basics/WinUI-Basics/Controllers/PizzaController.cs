using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using WinUI_Basics.Models;

namespace WinUI_Basics.Controllers
{
    class PizzaController
    {
        static public async Task<ObservableCollection<Pizza>> GetPizzas()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7114/api/Pizza/GetAll");
            request.Headers.Add("accept", "*/*");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            var pizzas = JsonSerializer.Deserialize<ObservableCollection<Pizza>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            if (pizzas == null)
            {
                return new ObservableCollection<Pizza>();
            }

            return pizzas;
        }

        static public async Task<bool> CreatePizza(Pizza pizza)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7114/api/Pizza/CreatePizza");
                request.Headers.Add("accept", "*/*");

                var jsonContent = JsonSerializer.Serialize(pizza, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        static public async Task<bool> EditPizza(Pizza pizza)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7114/api/Pizza/EditPizza");
                request.Headers.Add("accept", "*/*");

                var jsonContent = JsonSerializer.Serialize(pizza, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;

            }
            catch
            {
                return false;
            }
        }

        static public async Task<bool> DeletePizza(int id)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7114/api/Pizza/DeletePizza?id={id}");
                request.Headers.Add("accept", "*/*");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
