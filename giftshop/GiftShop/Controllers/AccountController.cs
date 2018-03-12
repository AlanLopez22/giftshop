using AutoMapper;
using GiftShop.Entities;
using GiftShop.Models;
using GiftShop.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GiftShop.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : BaseController
    {
        public AccountController(IRepository repository) : base(repository)
        {

        }

        [Route("authenticate"), HttpPost]
        public async Task<HttpResponseMessage> Authenticate(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;

                if (UserLogged != null)
                {
                    User user = await repository.FindByAsync<User>(f => f.ID == UserLogged.UserId);
                    UserModel model = Mapper.Map<User, UserModel>(user);
                    User = Thread.CurrentPrincipal;
                    user.Password = string.Empty;
                    response = request.CreateResponse(HttpStatusCode.OK, new
                    {
                        user = model
                    });
                }

                return await Task.FromResult(response);
            });
        }

        [Route("signup"), HttpPost, AllowAnonymous]
        public async Task<HttpResponseMessage> Signup(HttpRequestMessage request, SignupModel model)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;

                if (!model.Password.Equals(model.ConfirmPassword))
                    return request.CreateResponse(HttpStatusCode.BadRequest, new { error = "The passwords does not match." });

                UserType userType = await repository.FindByAsync<UserType>(f => f.Description.Equals("User", StringComparison.OrdinalIgnoreCase));
                User user = new User()
                {
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = true,
                    UserTypeID = userType.ID,
                    UserName = model.UserName,
                    Password = model.Password,
                    FirstName = "",
                    LastName = "",
                    Email = ""
                };
                repository.Create(user);
                await repository.SaveAsync<User>();
                user = await repository.FindByAsync<User>(f => f.UserName.Equals(model.UserName));
                response = request.CreateResponse(HttpStatusCode.OK, new
                {
                    user = Mapper.Map<User, UserModel>(user)
                });

                return await Task.FromResult(response);
            });
        }
    }
}
