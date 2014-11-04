using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPhotos.Services
{
    public class Image
    {
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public string WebUrl { get; set; }
        public string MobileUrl { get; set; }
        public string FullUrl { get; set; }
        public ImageMetadata ImageMetadata { get; set; }
        public Orientation? Orientation { get; set; }
        public void SetOrientation(string filePath)
        {
            if (Orientation.HasValue)
            {
                return;
            }
            System.Drawing.Image img = System.Drawing.Image.FromFile(filePath);
            if (img != null)
            {
                if (img.Width >= img.Height)
                {
                    this.Orientation = MyPhotos.Services.Orientation.Landscape;
                }
                else
                {
                    this.Orientation = MyPhotos.Services.Orientation.Portrait;
                }
            }
        }
    }
}