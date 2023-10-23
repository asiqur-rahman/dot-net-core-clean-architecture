using Microsoft.AspNetCore.Mvc;

namespace Project.Web.Controllers
{
    public class AccountController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
