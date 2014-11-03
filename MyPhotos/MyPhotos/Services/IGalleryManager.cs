using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotos.Services
{
    public enum GalleryOrder
    {
        CreateDate,
        LastUpdateDate,
        GalleryDate,
        Name,
    }
    public enum GalleryOrderDirection
    {
        Ascending,
        Descending,
    }

    public interface IGalleryManager
    {
        IGallery FindOrCreateGallery(string galleryName);
        IEnumerable<IGallery> ListGalleries();
        IOrderedEnumerable<IGallery> ListGalleries(GalleryOrder order, GalleryOrderDirection direction);
        void DeleteGallery(string galleryName);
        void UpdateGalleryMetadata(string galleryName, string galleryCaption, string galleryLocation, DateTime? galleryDate);
        void DeleteGalleryPhotos(string galleryName, List<string> photoNames);
        void UpdatePhotoCaptions(string galleryName, List<string> photoNames, List<string> captions);
    }
}
