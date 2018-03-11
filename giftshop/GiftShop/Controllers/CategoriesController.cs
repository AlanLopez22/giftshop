using AutoMapper;
using GiftShop.Entities;
using GiftShop.Models;
using GiftShop.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GiftShop.Controllers
{
    [RoutePrefix("api/category")]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IRepository repository) : base(repository)
        {

        }

        [Route("list"), HttpGet]
        public async Task<HttpResponseMessage> List(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                IEnumerable<Category> list = await repository.ListByAsync<Category>();
                IEnumerable<CategoryModel> listModel = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryModel>>(list.OrderBy(o => o.Description));
                response = request.CreateResponse(HttpStatusCode.OK, listModel);
                return await Task.FromResult(response);
            });
        }

    }
}
