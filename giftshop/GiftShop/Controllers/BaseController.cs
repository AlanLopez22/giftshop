using GiftShop.Filters;
using GiftShop.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GiftShop.Controllers
{
    public class BaseController : ApiController
    {
        protected IRepository repository;
        public BasicAuthenticationIdentity UserLogged { get; set; }

        public BaseController(IRepository repository)
        {
            this.repository = repository;
        }

        protected async Task<HttpResponseMessage> CreateHttpResponseAsync(HttpRequestMessage request, Func<Task<HttpResponseMessage>> function)
        {
            HttpResponseMessage response = null;

            try
            {
                if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    if (Thread.CurrentPrincipal.Identity is BasicAuthenticationIdentity basicAuthenticationIdentity)
                    {
                        UserLogged = basicAuthenticationIdentity;
                    }
                }

                response = await function.Invoke();
            }
            catch (Exception ex)
            {
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }
    }
}
