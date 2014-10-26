using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPhotos.Services
{
    public class SiteConfiguration : ISiteConfiguration
    {
        NameValueCollection settings;

        public SiteConfiguration()
        {
            settings = (NameValueCollection)ConfigurationManager.GetSection("MyPhotosSettings");
        }

        public string FaceBookAppId 
        { 
            get
            {
                return settings["FaceBookAppId"];
            }
        }
        public string FaceBookSecret
        { 
            get
            {
                return settings["FaceBookSecret"];
            }
        }
        public string BootstrapAdmins 
        {
            get
            {
                return settings["BootstrapAdmins"];
            }
        }

        public string GalleryFolderName
        {
            get
            {
                return settings["GalleryFolderName"];
            }
        }
    }
}