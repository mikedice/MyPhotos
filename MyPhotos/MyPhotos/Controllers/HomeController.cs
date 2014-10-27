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
            /*
            var galleries = galleryManager.ListGalleries(GalleryOrder.LastUpdateDate, GalleryOrderDirection.Descending);


            HomePageModel model = new HomePageModel();
            var viewGalleries = new List<ViewGallery>();

            var first = galleries.First();

            int imageCount = first.Images.Count();
            int numColumns = 0;
            int imagesPerColumn = 0; 

            if (imageCount <= MaxColumns)
            {
                imagesPerColumn = 1;
                numColumns = imageCount;
            }
            else
            {
                numColumns = MaxColumns;
                imagesPerColumn = imageCount / numColumns;
            }

            var images = first.Images.ToList();
            List<Image>[] columns = new List<Image>[numColumns];
            int imageIndex = 0;
            int imagesRemain = images.Count;
            for (int c = 0; c<columns.Length; c++)
            {
                columns[c] = new List<Image>();
                int imagesToAdd = Math.Min(imagesRemain, imagesPerColumn);
                for (int i =0; i<imagesToAdd; i++)
                {
                    columns[c].Add(images[imageIndex]);
                    imageIndex++;
                    imagesRemain--;
                }
            }

            ViewGallery vg = new ViewGallery
            {
                ImageColumns = columns,
                Gallery = first
            };
            viewGalleries.Add(vg);
            model.ViewGalleries = viewGalleries;
            return View(model);
             */
            return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}