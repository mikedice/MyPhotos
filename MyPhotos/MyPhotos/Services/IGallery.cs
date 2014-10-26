using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyPhotos.Services
{
    public interface IGallery
    {
        GalleryMetadata Metadata { get; }
        IEnumerable<Image> Images { get; }
        Task<bool> AddFileAsync(string fileName, Stream data, bool overwriteExisting);
        void UpdateImageMetadata(Image image);
        void UpdateGalleryMetadata(string galleryCaption, string galleryLocation, DateTime? galleryDate);
    }
}
