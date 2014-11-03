using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPhotos.Services;
using MyPhotos.Models;
using Newtonsoft.Json;

namespace MyPhotos.Controllers
{
    public class ContentController : Controller
    {
        private const int MaxColumns = 6;
        IGalleryManager galleryManager;

        public ContentController(IGalleryManager galleryManager)
        {
            this.galleryManager = galleryManager;
        }

        
        public ActionResult HomepageGalleries()
        {
            var galleries = galleryManager.ListGalleries(GalleryOrder.GalleryDate, GalleryOrderDirection.Descending);
            return ContentResultFromGalleryList(galleries);
        }

        public ActionResult HomepageGallery(string galleryName)
        {
            var galleries = galleryManager.ListGalleries(GalleryOrder.GalleryDate, GalleryOrderDirection.Descending);
            var selection = from g in galleries where g.Metadata.Name.Equals(galleryName, StringComparison.OrdinalIgnoreCase) select g;
            if (selection.Any())
            {
                return ContentResultFromGalleryList(selection);
            }
            return null;
        }
        private ActionResult ContentResultFromGalleryList(IEnumerable<IGallery> galleries)
        {
            HomePageModel model = new HomePageModel();
            var viewGalleries = new List<ViewGallery>();

            foreach (var gallery in galleries)
            {

                int imageCount = gallery.Images.Count();
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

                var images = gallery.Images.ToList();
                ImageColumn[] columns = new ImageColumn[numColumns];
                int imageIndex = 0;
                int imagesRemain = images.Count;
                for (int c = 0; c < columns.Length; c++)
                {
                    columns[c] = new ImageColumn()
                    {
                        Images = new List<Image>()
                    };

                    int imagesToAdd = Math.Min(imagesRemain, imagesPerColumn);
                    for (int i = 0; i < imagesToAdd; i++)
                    {
                        columns[c].Images.Add(images[imageIndex]);
                        imageIndex++;
                        imagesRemain--;
                    }
                }

                ViewGallery vg = new ViewGallery
                {
                    ImageColumns = columns.ToList<ImageColumn>(),
                    Gallery = gallery
                };
                viewGalleries.Add(vg);
            }
            model.ViewGalleries = viewGalleries;
            var json = JsonConvert.SerializeObject(model);
            var contentResult = new ContentResult()
            {
                Content = json,
                ContentType = "application/json",
                ContentEncoding = System.Text.Encoding.UTF8
            };
            return contentResult;

        }
    }
}