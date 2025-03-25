using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SD.Common.Services;
using SD.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SD.Application.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        protected readonly IUserService userService;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                          ILoggerFactory logger,
                                          UrlEncoder encoder,
                                          ISystemClock clock,
                                          IUserService userService) : base(options, logger, encoder, clock)
        {
            this.userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            {
                Response.Headers.Add(new KeyValuePair<string, StringValues>("WWW-Authenticate", "Basic realm=\"\""));
                return await Task.FromResult(AuthenticateResult.Fail("Authorization Header not found!"));
            }

            User user;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                /* Kodierte Base64 Zeichenfolge in Byte-Array umwandeln ([benutzername]:[passwort] */
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                // var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new char[] { ':' }, 2);
                /* Benutzername und Passwort aus dem Byte-Array extrahieren */
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split([':'], 2);                
                var username = credentials[0];
                var password = credentials[1];

                user = await this.userService.Authenticate(username, password, default);
            }
            catch
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header!"));
            }

            if(user == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password!"));
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.GivenName, user.FirstName)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
