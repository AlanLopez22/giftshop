using GiftShop.Repositories;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Dependencies;

namespace GiftShop.Infraestructure.Extentions
{
    public static class RequestMessageExtensions
    {
        public static IRepository GetDataRepository(this HttpRequestMessage request)
        {
            return request.GetService<IRepository>();
        }

        public static TService GetService<TService>(this HttpRequestMessage request)
        {
            IDependencyScope dependencyScope = request.GetDependencyScope();
            TService service = (TService)dependencyScope.GetService(typeof(TService));
            return service;
        }
        private static IEnumerable<TService> GetServices<TService>(this HttpRequestMessage request)
        {
            IDependencyScope dependencyScope = request.GetDependencyScope();
            IEnumerable<TService> services = (IEnumerable<TService>)dependencyScope.GetServices(typeof(TService));
            return services;
        }
    }
}