using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Project.Web.Hubs;
using Project.Web.Models;
using System.Diagnostics;

namespace Project.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignalRHubService _signalRHubService;
        public HomeController(ILogger<HomeController> logger, SignalRHubService signalRHubService)
        {
            _logger = logger;
            _signalRHubService = signalRHubService;
        }

        [HttpGet(Name = "HomeDashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            return View();
        }

        [HttpGet(Name = "HomePrivacy")]
        public async Task<IActionResult> Privacy()
        {
            await _signalRHubService.InvokeHubMethod("user", "ReceiveMessage", "Hello from user2");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}