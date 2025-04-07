using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WinUI_Basics.Views;

namespace WinUI_Basics.Controllers
{
    class ImageController
    {
        public static async Task<string> UploadImage(Windows.Storage.StorageFile file)
        {
            try
            {
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(stream), "file", file.Name);

                    var client = new HttpClient();
                    var response = await client.PostAsync("https://localhost:7114/api/Image/UploadImage", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<UploadImageResponse>();
                        string uploadedImageUrl = result.Url;
                        return uploadedImageUrl;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
