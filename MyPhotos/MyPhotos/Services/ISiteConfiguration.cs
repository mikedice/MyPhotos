using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPhotos.Services
{
    public interface ISiteConfiguration
    {
        string FaceBookAppId { get; }
        string FaceBookSecret { get; }
        string BootstrapAdmins { get; }
        string GalleryFolderName { get;  }
    }
}