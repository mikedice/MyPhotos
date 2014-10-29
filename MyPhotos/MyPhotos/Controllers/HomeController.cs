using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using MyPhotos.Services;
using MyPhotos.Models;

namespace MyPhotos.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private const int MaxColumns = 6;
        IGalleryManager galleryManager;

        public HomeController(IGalleryManager galleryManager)
        {
            this.galleryManager = galleryManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}