using Frontend.Models;
using Frontend.Services;
using Newtonsoft.Json;
using System.Text;

namespace Frontend.Service
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    //    public async Task<string> Register(Supplier product)
    //    {
    //        try
    //        {
    //            var url = "https://localhost:7028/register";
    //            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
    //            var response = await _httpClient.PutAsync(url, content);

    //            if (response.IsSuccessStatusCode)
    //                return "done";
    //            else
    //                return $"Failed with status code: {response.StatusCode}";
    //        }
    //        catch (Exception ex)
    //        {
    //            return "Error";
    //        }
    //    }

    //    public async Task<Supplier> Login()
    //    {
    //        try
    //        {
    //            var url = "https://localhost:7028/ProjectService/GetData";
    //            var response = await _httpClient.GetAsync(url);

    //            if (response.IsSuccessStatusCode)
    //            {
    //                var data = await response.Content.ReadAsAsync<List<Product>>();
    //                return data;
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return null;
    //        }
    //    }
    }

}
