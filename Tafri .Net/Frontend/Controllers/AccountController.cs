namespace Frontend.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using Frontend.Models;

    public class AccountController : Controller
    {
    //    private readonly SignInManager<IdentityUser> _signInManager;

    //    public AccountController(SignInManager<IdentityUser> signInManager)
    //    {
    //        _signInManager = signInManager;
    //    }

    //    [HttpGet]
    //    public IActionResult Login()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Login(Supplier model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            var result = await _signInManager.PasswordSignInAsync(model.SupplierEmail, model.SupplierPassword, false);

    //            if (result.Succeeded)
    //            {
    //                return RedirectToAction("Index", "Home");
    //            }

    //            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
    //        }

    //        return View(model);
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Logout()
    //    {
    //        await _signInManager.SignOutAsync();
    //        return RedirectToAction("Index", "Home");
    //    }
    }

}
