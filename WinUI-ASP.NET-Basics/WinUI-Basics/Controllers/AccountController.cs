using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WinUI_Basics.Models;

namespace WinUI_Basics.Controllers
{
    class AccountController
    {
        static public async Task<bool> CheckUserCredentials(string email, string password)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7114/api/User/Login?email={email}&password={password}");
                request.Headers.Add("accept", "text/plain");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        static public async Task<bool> RegisterUser(User user)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7114/api/User/Register");
                request.Headers.Add("accept", "*/*");
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
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
