using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPhotos.Services
{
    public class Image
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public string ThumbnailUrl { get; set; }
        public string WebUrl { get; set; }
        public string MobileUrl { get; set; }
        public string FullUrl { get; set; }
        public ImageMetadata ImageMetadata { get; set; }
    }
}