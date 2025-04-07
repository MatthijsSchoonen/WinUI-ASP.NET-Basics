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
    class AccountController
    {
        static public async Task<bool> CheckUserCredentials(string email, string password)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7114/api/User/Login?email={email}&password={password}");
                request.Headers.Add("accept", "application/json");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    MainWindow._LoggedInUser = user;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        static public async Task<ObservableCollection<User>> GetUsers()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7114/api/User/GetAllUsers");
                request.Headers.Add("accept", "application/json");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return new ObservableCollection<User>(users);
                }
                return new ObservableCollection<User>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<User>();
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


        static public async Task<bool> EditUser(User user)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7114/api/User/UpdateUser");
                request.Headers.Add("accept", "application/json");
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
            catch (Exception ex)
            {
                return false;
            }
        }


    static public async Task<bool> UpdatePassword(int id, string pass)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:7114/api/User/UpdatePassword?id={id}&password={pass}");
                request.Headers.Add("accept", "text/plain");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    MainWindow._LoggedInUser = user;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        static public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7114/api/User/DeleteUser?id={id}");
                request.Headers.Add("accept", "*/*");
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
