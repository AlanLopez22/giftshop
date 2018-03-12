using AutoMapper;
using GiftShop.Entities;
using GiftShop.Models;
using GiftShop.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GiftShop.Controllers
{
    [RoutePrefix("api/order")]
    public class OrdersController : BaseController
    {
        public OrdersController(IRepository repository) : base(repository)
        {

        }

        [Route("get/{id:int=0}/"), HttpGet, AllowAnonymous]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage request, int id)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                Status status = await repository.FindByAsync<Status>(f => f.Description.Equals("Pending", StringComparison.OrdinalIgnoreCase));
                StatusModel statusModel = Mapper.Map<Status, StatusModel>(status);
                Order order = null;

                if (UserLogged != null && id == 0)
                    order = await repository.FindByAsync<Order>(f => f.User.UserName == UserLogged.UserName && f.StatusID == status.ID);
                else
                    order = await repository.FindByAsync<Order>(f => f.ID == id);

                if (order == null)
                {
                    order = new Order
                    {
                        CreatedOn = DateTime.Now,
                        StatusID = statusModel.ID
                    };

                    if (UserLogged != null)
                        order.UserID = UserLogged.UserId;
                }

                OrderModel orderModel = Mapper.Map<Order, OrderModel>(order);
                response = request.CreateResponse(HttpStatusCode.OK, orderModel);
                return await Task.FromResult(response);
            });
        }

        [Route("addproduct"), HttpPost, AllowAnonymous]
        public async Task<HttpResponseMessage> AddProduct(HttpRequestMessage request, OrderDetailModel model)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                Status status = await repository.FindByAsync<Status>(f => f.Description.Equals("Pending", StringComparison.OrdinalIgnoreCase));
                StatusModel statusModel = Mapper.Map<Status, StatusModel>(status);
                Order order = null;
                
                if (UserLogged != null)
                    order = await repository.FindByAsync<Order>(f => f.User.UserName == UserLogged.UserName && f.StatusID == status.ID);
                else
                    order = await repository.FindByAsync<Order>(f => f.ID == model.OrderID);

                if (order == null)
                {
                    order = new Order
                    {
                        CreatedOn = DateTime.Now,
                        StatusID = statusModel.ID
                    };

                    if (UserLogged != null)
                        order.UserID = UserLogged.UserId;

                    repository.Create(order);
                    await repository.SaveAsync<Order>();
                }

                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderID = order.ID,
                    Price = model.Product.Price,
                    ProductID = model.ProductID,
                    Quantity = model.Quantity
                };
                repository.Create(orderDetail);
                await repository.SaveAsync<OrderDetail>();
                order.Amount = order.OrderDetails.Sum(s => s.Price);
                repository.Update(order);
                await repository.SaveAsync<Order>();

                if(orderDetail.Product == null)
                    orderDetail.Product = await repository.FindByAsync<Product>(f => f.ID == orderDetail.ProductID);

                OrderModel orderModel = Mapper.Map<Order, OrderModel>(order);
                response = request.CreateResponse(HttpStatusCode.OK, orderModel);
                return await Task.FromResult(response);
            });
        }

        [Route("removeproduct"), HttpPost, AllowAnonymous]
        public async Task<HttpResponseMessage> RemoveProduct(HttpRequestMessage request, OrderDetailModel model)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                Order order = await repository.FindByAsync<Order>(f => f.ID == model.OrderID);

                if (order == null)
                    throw new Exception("Could not find order with ID: " + model.OrderID);

                OrderDetail orderDetail = await repository.FindByIDAsync<OrderDetail>(model.ID);

                if (orderDetail == null)
                    throw new Exception("Could not find the product on order ID: " + model.ID);

                repository.Delete(orderDetail);
                await repository.SaveAsync<OrderDetail>();
                order.Amount = order.OrderDetails.Sum(s => s.Price);
                repository.Update(order);
                await repository.SaveAsync<Order>();
                OrderModel orderModel = Mapper.Map<Order, OrderModel>(order);
                response = request.CreateResponse(HttpStatusCode.OK, orderModel);
                return await Task.FromResult(response);
            });
        }

        [Route("confirmorder"), HttpPost]
        public async Task<HttpResponseMessage> ConfirmOrder(HttpRequestMessage request, OrderModel model)
        {
            return await CreateHttpResponseAsync(request, async () =>
            {
                HttpResponseMessage response = null;
                Order order = await repository.FindByAsync<Order>(f => f.ID == model.ID);

                if (order == null)
                    throw new Exception("Could not find order with ID: " + model.ID);

                Status status = await repository.FindByAsync<Status>(f => f.Description.Equals("Confirmed", StringComparison.OrdinalIgnoreCase));
                StatusModel statusModel = Mapper.Map<Status, StatusModel>(status);
                order.StatusID = status.ID;
                order.UserID = UserLogged.UserId;
                repository.Update(order);
                await repository.SaveAsync<Order>();
                OrderModel orderModel = Mapper.Map<Order, OrderModel>(order);
                response = request.CreateResponse(HttpStatusCode.OK, orderModel);
                return await Task.FromResult(response);
            });
        }
    }
}
