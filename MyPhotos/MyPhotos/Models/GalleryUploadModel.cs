using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MyPhotos.Models
{
    public class GalleryUploadModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required]
        public IEnumerable<HttpPostedFileBase> files { get; set; }
    }
}