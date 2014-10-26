using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPhotos.Services
{
    public class GalleryMetadata
    {
        public string Name { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime Created { get; set; }
        public string GalleryCaption { get; set; }
        public string GalleryLocation { get; set; }
        public DateTime? GalleryDate { get; set; }
    }
}