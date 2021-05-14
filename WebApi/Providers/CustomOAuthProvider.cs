using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using WebApi.Stores;

namespace WebApi.Models
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public CustomOAuthProvider()
        {
        } 
        
        public CustomOAuthProvider(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
            UserManager = userManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Validates Client
        /// </summary>
        /// <param name="context">Client Authenticatiocn Context</param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }

            var audience = AudiencesStore.FindAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Grants Resource Owner Credentials.
        /// </summary>
        /// <param name="context">GrantResourceOwnerCredentialsContext</param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            
            // Check Users exists and is valid to grant credentials
            var result = await SignInManager.PasswordSignInAsync(context.UserName, context.Password, false, false);

            // If signing failure return failure message.
            if (result == SignInStatus.Failure)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect");
                return;
                //return Task.FromResult<object>(null);
            }
            // If Signing successfull then create and authentication ticket.
            if(result == SignInStatus.Success)
            {
                var user = await UserManager.FindByNameAsync(context.UserName);
                var roles = await UserManager.GetRolesAsync(user.Id);
                var identity = new ClaimsIdentity("JWT");

                identity.AddClaim(new Claim("sub", user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                foreach (var role in roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
                }

                var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        {
                             "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                        }
                    });

                var ticket = new AuthenticationTicket(identity, props);
                context.Validated(ticket);
            }
            return;
        }
    }
}