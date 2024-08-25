using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.App.Hubs;
using Project.Core.Config;
using Project.Core.Entities.Common.Account.Dtos;
using Project.Core.Entities.Common.Security;
using Serilog;
using System.Security.Claims;
using System.Text.Json;

namespace Project.App.Controllers
{
    public class AccountController : BaseController
    {
        public readonly IConfiguration _configuration;
        public readonly AppSettings _appSettings;
        private readonly SignalRHubService _signalRHubService;
        public AccountController(IConfiguration configuration, IOptions<AppSettings> appSettings
            , SignalRHubService signalRHubService
            )
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
            _signalRHubService = signalRHubService;
        }

        public IActionResult Login()
        {
            Log.Debug("Account Controller View !");
            Log.Information("Account Controller View !");
            Log.Warning("Account Controller View !");
            Log.Error("Account Controller View !");
            Log.Fatal("Account Controller View !");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl = "/Home/Dashboard")
        {
            string cookieName = _appSettings.Cookie.Name;

            var claims = new List<Claim> {
                            new Claim(ClaimTypes.Name, model.Username, ClaimValueTypes.String, cookieName)
                        };

            var userIdentity = new ClaimsIdentity(claims, cookieName);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(cookieName, userPrincipal, new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddSeconds(_appSettings.Cookie.Expires),
                IsPersistent = false,
                AllowRefresh = true
            });

            UserPrincipal cookiesData = new UserPrincipal();
            cookiesData.Username = model.Username;
            HttpContext.Session.SetString("UserSessionData", JsonSerializer.Serialize(cookiesData));
            if (_signalRHubService.IsUserConnected(model.Username))
            {
                await _signalRHubService.InvokeHubMethod(model.Username, "LogOut");
            }
            return RedirectPermanent(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(_appSettings.Cookie.Name);
            return RedirectToAction("Login");
        }
    }
}
