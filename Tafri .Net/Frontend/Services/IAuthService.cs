using Microsoft.AspNetCore.Mvc;

namespace Frontend.Services
{
    public class IAuthService : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
