using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Threading.Tasks;
using System.IO;

namespace MyPhotos.Services
{
    public class GalleryManager : IGalleryManager
    {
        private ISiteConfiguration siteConfig;
        private string galleryRootPath;

        public GalleryManager(ISiteConfiguration siteConfig)
        {
            this.siteConfig = siteConfig;
            if (HostingEnvironment.IsHosted)
            {
                galleryRootPath = HostingEnvironment.MapPath("/" + siteConfig.GalleryFolderName);
            }
            else
            {
                galleryRootPath = Environment.CurrentDirectory + siteConfig.GalleryFolderName;
            }
            if (!Directory.Exists(galleryRootPath))
            {
                Directory.CreateDirectory(galleryRootPath);
            }
        }

        public void UpdateGalleryMetadata(string galleryName, string galleryCaption, string galleryLocation, DateTime? galleryDate)
        {
            var gallery = FindGallery(galleryName);
            if (gallery != null)
            {
                gallery.UpdateGalleryMetadata(galleryCaption, galleryLocation, galleryDate);
            }
        }

        public void DeleteGallery(string galleryName)
        {
            var galleryFileRoot = Path.Combine(this.galleryRootPath, galleryName);
            if (Directory.Exists(galleryFileRoot))
            {
                Directory.Delete(galleryFileRoot, true);
            }
        }
        public void DeleteGalleryPhotos(string galleryName, List<string> photoNames)
        {
            var galleryFileRoot = Path.Combine(this.galleryRootPath, galleryName);
            if (Directory.Exists(galleryFileRoot))
            {
                foreach(string photo in photoNames)
                {
                    var files = Directory.EnumerateFiles(galleryFileRoot, photo + "*");
                    foreach(var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        public void UpdatePhotoCaptions(string galleryName,
            List<string> photoNames, 
            List<string> captions)
        {
            var gallery = FindGallery(galleryName);
            for (int i = 0; i < photoNames.Count; i++)
            {
                var selection = from img in gallery.Images where img.Name.Equals(photoNames[i]) select img;
                if (selection.Any())
                {
                    var image = selection.First();
                    image.ImageMetadata.Caption = captions[i];
                    gallery.UpdateImageMetadata(image);
                }
            }
        }

        private IGallery FindGallery(string galleryName)
        {
            IGallery result = null;
            var galleryFileRoot = Path.Combine(this.galleryRootPath, galleryName);
            if (!Directory.Exists(galleryFileRoot))
            {
                return null;
            }
            result = Gallery.CreateAsync(galleryName, galleryFileRoot, siteConfig.GalleryFolderName);
            return result;
        }

        public IGallery FindOrCreateGallery(string galleryName)
        {
            IGallery result = null;
            var galleryFileRoot = Path.Combine(this.galleryRootPath, galleryName);
            if (!Directory.Exists(galleryFileRoot))
            {
                Directory.CreateDirectory(galleryFileRoot);
            }

            result = Gallery.CreateAsync(galleryName, galleryFileRoot, siteConfig.GalleryFolderName);
            return result;
        }

        public IEnumerable<IGallery> ListGalleries()
        {
            List<IGallery> galleries = new List<IGallery>();
            var directories = Directory.EnumerateDirectories(galleryRootPath);
            foreach(var directory in directories)
            {
                var metaPath = Path.Combine(Path.Combine(galleryRootPath, directory), Gallery.MetadataFileName);
                if (File.Exists(metaPath))
                {
                    var gallery = Gallery.CreateAsync(directory, Path.Combine(galleryRootPath, directory), siteConfig.GalleryFolderName);
                    galleries.Add(gallery);
                }
            }

            return galleries;
        }

        public IOrderedEnumerable<IGallery> ListGalleries(GalleryOrder order, GalleryOrderDirection direction)
        {
            IOrderedEnumerable <IGallery> result = null;
            var galleries = this.ListGalleries();
            if (direction == GalleryOrderDirection.Ascending)
            {
                if (order == GalleryOrder.Name)
                {
                    result = galleries.OrderBy(g => g.Metadata.Name);
                }
                else if (order == GalleryOrder.CreateDate)
                {
                    result = galleries.OrderBy(g => g.Metadata.Created);
                }
                else if (order == GalleryOrder.LastUpdateDate)
                {
                    result = galleries.OrderBy(g => g.Metadata.LastUpdate);
                }
            }
            else if (direction == GalleryOrderDirection.Descending)
            {
                if (order == GalleryOrder.Name)
                {
                    result = galleries.OrderByDescending(g => g.Metadata.Name);
                }
                else if (order == GalleryOrder.CreateDate)
                {
                    result = galleries.OrderByDescending(g => g.Metadata.Created);
                }
                else if (order == GalleryOrder.LastUpdateDate)
                {
                    result = galleries.OrderByDescending(g => g.Metadata.LastUpdate);
                }
            }
            return result;
        }
    }
}