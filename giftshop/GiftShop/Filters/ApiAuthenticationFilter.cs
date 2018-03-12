using GiftShop.Entities;
using GiftShop.Infraestructure.Extentions;
using GiftShop.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http.Controllers;

namespace GiftShop.Filters
{
    /// <summary>
    /// Custom Authentication Filter Extending basic Authentication
    /// </summary>
    public class ApiAuthenticationFilter : AppAuthenticationFilter
    {
        /// <summary>
        /// Default Authentication Constructor
        /// </summary>
        public ApiAuthenticationFilter()
        {
        }

        /// <summary>
        /// AuthenticationFilter constructor with isActive parameter
        /// </summary>
        /// <param name="isActive"></param>
        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        /// <summary>
        /// Protected overriden method for authorizing user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            bool success = base.OnAuthorizeUser(username, password, actionContext);

            if (!success)
                return success;

            List<string> errors = new List<string>();
            IRepository repository = actionContext.Request.GetDataRepository();
            User user = repository.FindBy<User>(f => f.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                if (string.Equals(password, user.Password))
                {
                    if (Thread.CurrentPrincipal.Identity is BasicAuthenticationIdentity basicAuthenticationIdentity)
                        basicAuthenticationIdentity.UserId = user.ID;

                    return true;
                }
                else
                    errors.Add($"Invalid Password");
            }
            else
                errors.Add($"User \"{username}\" not found");

            if (errors.Count > 0)
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { errors });

            return false;
        }
    }
}