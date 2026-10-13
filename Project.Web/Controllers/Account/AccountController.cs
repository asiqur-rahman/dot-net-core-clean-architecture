using Microsoft.AspNetCore.Mvc;
using Project.Core.Entities.Common.Account.Dtos;

namespace Project.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginDto model)
        {
            if (ModelState.IsValid && model.Username == "admin" && model.Password == "admin")
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }
    }
}