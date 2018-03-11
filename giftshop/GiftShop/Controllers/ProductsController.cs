using AutoMapper;
using GiftShop.Entities;
using GiftShop.Infraestructure;
using GiftShop.Models;
using GiftShop.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GiftShop.Controllers
{
    [RoutePrefix("api/product")]
    public class ProductsController : BaseController
    {
        public ProductsController(IRepository repository) : base(repository)
        {

        }

        [Route("list/{page:int=0}/{pageSize:int=50}/{categoryID:int=0}/{name?}/"), HttpGet]
        public async Task<HttpResponseMessage> List(HttpRequestMessage request, int page, int pageSize, int categoryID, string name = null)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                IEnumerable<Product> list = await repository.ListByAsync<Product>(w => (categoryID == 0 || w.CategoryID == categoryID) && (string.IsNullOrEmpty(name) || w.Name.ToUpper().Contains(name.ToUpper())));
                IEnumerable<ProductModel> listModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductModel>>(list.OrderBy(o => o.Name));
                PaginationSet<ProductModel> pagedSet = listModel.ToPagedList(page, list.Count(), pageSize);
                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return await Task.FromResult(response);
            });
        }

        [Route("getByID/{id:int=0}/"), HttpGet]
        public async Task<HttpResponseMessage> GetByID(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                Product product = await repository.FindByIDAsync<Product>(id);
                ProductModel model = Mapper.Map<Product, ProductModel>(product);
                response = request.CreateResponse(HttpStatusCode.OK, model);
                return await Task.FromResult(response);
            });
        }

        [Route("Save"), HttpPost]
        public async Task<HttpResponseMessage> Save(HttpRequestMessage request, ProductModel viewModel)
        {
            return await CreateHttpResponseAsync(request, async () => {
                HttpResponseMessage response = null;
                Product entity = await repository.FindByIDAsync<Product>(viewModel.ID);

                if (entity == null)
                {
                    entity = new Product
                    {
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    entity.Update(viewModel);
                    repository.Create(entity);
                }
                else
                {
                    entity.UpdatedOn = DateTime.Now;
                    entity.Update(viewModel);
                    repository.Update(entity);
                }

                await repository.SaveAsync<Product>();

                if (viewModel.Images != null)
                {
                    int[] ids = viewModel.Images.Select(s => s.ID).ToArray();
                    IEnumerable<ProductImage> productImages = await repository.ListByAsync<ProductImage>(l => l.ProductID == entity.ID && !ids.Contains(l.ID));
                    int totalImages = productImages != null ? productImages.Count() - 1 : -1;

                    foreach (var image in viewModel.Images.Where(w => !w.ImagePath.Equals("./Images/noimage.png", StringComparison.OrdinalIgnoreCase)))
                    {
                        ProductImage productImage = await repository.FindByAsync<ProductImage>(l => l.ID == image.ID);

                        if (productImage == null)
                        {
                            productImage = new ProductImage { ProductID = entity.ID, ID = -1, ImagePath = image.ImagePath };
                            repository.Create(productImage);
                        }
                        else
                        {
                            if (!productImage.ImagePath.Equals(image.ImagePath, StringComparison.OrdinalIgnoreCase))
                            {
                                string filename = Path.Combine(HttpContext.Current.Server.MapPath("~"), productImage.ImagePath);
                                FileInfo file = new FileInfo(filename);

                                if (file.Exists)
                                    file.Delete();
                            }

                            productImage.ImagePath = image.ImagePath;
                            repository.Update(productImage);
                        }
                    }

                    for (int i = totalImages; i >= 0; i--)
                    {
                        ProductImage productImage = productImages.ElementAtOrDefault(i);
                        repository.Delete(productImage);
                    }

                    await repository.SaveAsync<ProductImage>();
                }

                response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                return response;
            });
        }

        [Route("Delete"), HttpPost]
        public async Task<HttpResponseMessage> Delete(HttpRequestMessage request, ProductModel viewModel)
        {
            return await CreateHttpResponseAsync(request, async () => {
                HttpResponseMessage response = null;
                Product entity = await repository.FindByIDAsync<Product>(viewModel.ID);

                if (entity == null)
                    throw new Exception("Could not find product with ID " + viewModel.ID);
                else
                {
                    entity.UpdatedOn = DateTime.Now;
                    entity.IsActive = false;
                }

                await repository.SaveAsync<Product>();
                response = request.CreateResponse(HttpStatusCode.OK, new { success = true });
                return response;
            });
        }

        [Route("UploadImage")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadImage(HttpRequestMessage request)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                string imagePath = "/Images/Products/";
                string directory = HttpContext.Current.Server.MapPath("~" + imagePath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var provider = new UploadMultipartFormProvider(directory);
                await Request.Content.ReadAsMultipartAsync(provider);
                MultipartFileData image = provider.FileData.FirstOrDefault();
                FileInfo file = new FileInfo(image.LocalFileName);

                if (file.Exists)
                {
                    Guid imageGuid = Guid.NewGuid();
                    string imageName = imageGuid + file.Extension;
                    file.MoveTo(Path.Combine(directory, imageName));
                    response = request.CreateResponse(HttpStatusCode.OK, new { image = $".{imagePath}{imageName}", success = true });
                }
                else
                    response = request.CreateResponse(HttpStatusCode.OK, new { success = false });

                return await Task.FromResult(response);
            });
        }
    }
}
