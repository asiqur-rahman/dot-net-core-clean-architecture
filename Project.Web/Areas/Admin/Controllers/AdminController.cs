using Microsoft.AspNetCore.Mvc;

namespace Project.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
