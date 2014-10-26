using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyPhotos.Models;
using System.Threading.Tasks;
using MyPhotos.Services;
namespace MyPhotos.Controllers
{
    public class PhotoManagerController : Controller
    {
        IGalleryManager galleryManager;
        ISiteConfiguration siteConfig;

        public PhotoManagerController(IGalleryManager galleryManager, ISiteConfiguration siteConfig)
        {
            this.galleryManager = galleryManager;
            this.siteConfig = siteConfig;
        }

        // GET: PhotoManager
        [Authorize(Roles = ApplicationUserManager.AdminRoleName)]
        public ActionResult Index()
        {
            var galleries = this.galleryManager.ListGalleries(GalleryOrder.LastUpdateDate, GalleryOrderDirection.Descending);

            return View(galleries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationUserManager.AdminRoleName)]
        public async Task<ActionResult> Create([Bind(Include = "Name, files")]GalleryUploadModel model)
        {
            if (ModelState.IsValid)
            {
                // Find the gallery object
                IGallery gallery = galleryManager.FindOrCreateGallery(model.Name);
                if (gallery != null)
                {
                    foreach (var file in model.files)
                    {
                        if (file != null)
                        {
                            await gallery.AddFileAsync(file.FileName, file.InputStream, true);
                        }
                    }
                }
            }

            var galleries = this.galleryManager.ListGalleries(GalleryOrder.LastUpdateDate, GalleryOrderDirection.Descending);
            return View("Index", galleries);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = ApplicationUserManager.AdminRoleName)]
        public ActionResult Update(string galleryName, 
            string galleryCaption,
            string galleryLocation,
            DateTime? galleryDate,
            string deleteGallery,
            List<string> photoDelete,
            List<string> photoName, 
            List<string> photoCaption)
        {
            if (!string.IsNullOrEmpty(deleteGallery))
            {
                this.galleryManager.DeleteGallery(deleteGallery);
                return View("Index", this.galleryManager.ListGalleries(
                    GalleryOrder.LastUpdateDate, 
                    GalleryOrderDirection.Descending));
            }

            this.galleryManager.UpdateGalleryMetadata(galleryName, galleryCaption, galleryLocation, galleryDate);

            if (photoDelete != null && photoDelete.Count > 0)
            {
                this.galleryManager.DeleteGalleryPhotos(galleryName, photoDelete);
            }
            if (photoName != null && photoCaption != null)
            {
                this.galleryManager.UpdatePhotoCaptions(galleryName, photoName, photoCaption);
            }

            var galleries = this.galleryManager.ListGalleries(GalleryOrder.LastUpdateDate, GalleryOrderDirection.Descending);

            return View("Index", galleries);
        }
    }


}