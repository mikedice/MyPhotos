using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using MyPhotos.Services;
using MyPhotos.Controllers;
namespace MyPhotos
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ISiteConfiguration, SiteConfiguration>();
            container.RegisterType<IGalleryManager, GalleryManager>();
            container.RegisterType<AccountController>(new InjectionConstructor());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}