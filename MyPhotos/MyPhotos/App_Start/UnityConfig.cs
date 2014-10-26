using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using MyPhotos.Services;

namespace MyPhotos
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ISiteConfiguration, SiteConfiguration>();
            container.RegisterType<IGalleryManager, GalleryManager>();


            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}