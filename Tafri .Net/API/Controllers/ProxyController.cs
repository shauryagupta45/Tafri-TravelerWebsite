using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Controller]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ProxyController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("")]
        public async Task<IActionResult> GenerateOrder([FromBody] GenerateOrderRequest request)
        {
            var targetUrl = "http://localhost:7000/generateOrder"; // URL of the API you want to call

            // Create a request message to forward to the target API
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, targetUrl)
            {
                Content = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json")
            };

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);

                // Check if the response is successful
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Content(content, "application/json");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                // Log the exception (for production use, consider using a logging framework)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    // Define the request model for generateOrder
    public class GenerateOrderRequest
    {
        public string Amount { get; set; }
        //public string UserId { get; set; }
        public string BookingId { get; set; }
    }
}
