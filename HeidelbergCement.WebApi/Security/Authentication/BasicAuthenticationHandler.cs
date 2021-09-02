using HeidelbergCement.Data.Models;
using HeidelbergCement.Service.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HeidelbergCement.WebApi.Security.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly IUserRepository<User> _userRepository;
        private const string AuthorizationHeaderName = "Authorization";
        private const string BasicSchemeName = "Basic";

        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserRepository<User> userRepository)
        : base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationHeaderName))
            {
                // Authorization header not found.
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[AuthorizationHeaderName],
                out AuthenticationHeaderValue headerValue))
            {
                // Authorization header is not valid.
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (!BasicSchemeName.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                // Authorization header is not Basic.
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // Extract username and password.
            byte[] headerValueBytes = Convert.FromBase64String(headerValue.Parameter);
            string userPassword = Encoding.UTF8.GetString(headerValueBytes);

            string[] parts = userPassword.Split(':');

            if (parts.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Basic Authentication Header"));
            }

            string username = parts[0];
            string password = parts[1];

            // Validate if username and password are correct.
            var user = _userRepository.GetUsersAsync().Result.SingleOrDefault(x => x.Name == username && x.Password == password);

            if (user == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid username or password."));
            }

            // Create claims with username and id.
            var claims = new[]
            {   new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"Basic realm=\"https://localhost:5000\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
    }
}
