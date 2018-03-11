using GiftShop.Entities;
using GiftShop.Infraestructure.Extentions;
using GiftShop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace GiftShop.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AppAuthenticationFilter : AuthorizationFilterAttribute
    {
        private readonly bool _isActive = true;

        /// <summary>
        /// Public default Constructor
        /// </summary>
        public AppAuthenticationFilter()
        {

        }

        /// <summary>
        /// parameter isActive explicitly enables/disables this filetr.
        /// </summary>
        /// <param name="isActive"></param>
        public AppAuthenticationFilter(bool isActive)
        {
            _isActive = isActive;
        }

        /// <summary>
        /// Checks basic authentication request
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (SkipAuthorization(filterContext))
                return;

            if (!_isActive)
                return;
            
            var identity = FetchAuthHeader(filterContext, out List<string> errors);

            if (identity == null)
            {
                filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { errors });
                return;
            }

            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;

            if (!OnAuthorizeUser(identity.Name, identity.Password, filterContext))
                return;

            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// Virtual method. Can be overriden with the custom Authorization.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext filterContext)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                List<string> errors = new List<string>();

                if (string.IsNullOrEmpty(username))
                    errors.Add("User name is required.");

                if (string.IsNullOrEmpty(password))
                    errors.Add("Password is required.");

                if (errors.Count > 0)
                    filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { errors });

                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks for autrhorization header in the request and parses it, creates user credentials and returns as BasicAuthenticationIdentity
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext filterContext, out List<string> errors)
        {
            errors = new List<string>();
            string authHeaderValue = null;
            var authRequest = filterContext.Request.Headers.Authorization;

            if (authRequest != null && !String.IsNullOrEmpty(authRequest.Scheme) && authRequest.Scheme == "Basic")
                authHeaderValue = authRequest.Parameter;

            if (string.IsNullOrEmpty(authHeaderValue))
            {
                errors.Add("Authorization scheme header value not found.");
                return null;
            }

            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));
            var credentials = authHeaderValue.Split(':');
            BasicAuthenticationIdentity basicAuthentication = null;

            try
            {
                if (credentials.Length >= 2)
                {
                    IRepository repository = filterContext.Request.GetService<IRepository>();
                    string userName = credentials[0];
                    User user = repository.FindBy<User>(f => f.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

                    if (user != null)
                        basicAuthentication = new BasicAuthenticationIdentity(user.UserName, credentials[1], user.UserType.Description);
                    else
                        errors.Add($"User name \"{userName}\" not found.");
                }
            }
            catch(Exception ex)
            {
                errors.Add(ex.Message);
            }

            return basicAuthentication;
        }
        
        private bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}