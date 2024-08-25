using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace Project.App.Handler
{
    public class SejilAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string Username = "admin";  // Replace with your specific username
        private const string Password = "password";  // Replace with your specific password

        public SejilAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                var authHeaderVal = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme != "Basic")
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Scheme"));

                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderVal.Parameter)).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                // Validate the credentials
                if (username == Username && password == Password)
                {
                    var claims = new[] {
                    new Claim(ClaimTypes.Name, username)
                };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
                }
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }
        }
    }
}
