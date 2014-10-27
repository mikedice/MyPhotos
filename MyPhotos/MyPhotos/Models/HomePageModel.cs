using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyPhotos.Services;

namespace MyPhotos.Models
{
    public class ImageColumn
    {
        public List<Image> Images { get; set; }
    }

    public class ViewGallery
    {
        public List<ImageColumn> ImageColumns { get; set; }
        public IGallery Gallery { get; set; }
    }

    public class HomePageModel
    {
        public IEnumerable<ViewGallery> ViewGalleries { get; set; }
    }
}