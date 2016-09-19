using Microsoft.Practices.Unity;
using MyListApp.Api.Data.Context;
using MyListApp.Api.Services;
using System.Web.Http;
using Unity.WebApi;

namespace MyListApp.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration config)
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IListRepository, ListRepository>();

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}