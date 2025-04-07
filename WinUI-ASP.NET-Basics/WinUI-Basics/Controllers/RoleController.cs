using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WinUI_Basics.Models;

namespace WinUI_Basics.Controllers
{
    class RoleController
    {
        static public async Task<ObservableCollection<Role>> GetRoles()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7114/api/Role/GetallRoles");
                request.Headers.Add("accept", "text/plain");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var roles = await response.Content.ReadFromJsonAsync<List<Role>>();
                    return new ObservableCollection<Role>(roles);
                }
                return new ObservableCollection<Role>();
            }
            catch (Exception ex)
            {
                return new ObservableCollection<Role>();
            }
        }

    }
}
