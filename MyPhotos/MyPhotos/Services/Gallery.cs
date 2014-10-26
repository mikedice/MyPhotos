using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyPhotos.Services
{
    public class Gallery : IGallery
    {
        public const string MetadataFileName = "gallery.json";
        private const string ThumbFileSuffix = "*_thumb.*";
        private const string WebFileSuffix = "*_web.*";
        private const string MobileFileSuffix = "*_mob.*";
        private const string FullFileSuffix = "*_full.*";
        private const string ImageMetadataSuffix = ".metadata.json";

        private string galleryFileRoot;
        private GalleryMetadata galleryMetadata;

        private Gallery(string galleryFileRoot)
        {
            this.galleryFileRoot = galleryFileRoot;
        }

        public GalleryMetadata Metadata
        {
            get
            {
                return this.galleryMetadata;
            }
        }

        public IEnumerable<Image> Images { get; private set; }

        public static IGallery CreateAsync(string name, string galleryFileRoot, string galleryWebRoot)
        {
            var result = new Gallery(galleryFileRoot);
            result.LoadMetadata(name);
            result.LoadImages(galleryWebRoot, name);
            return result;
        }

        public async Task<bool> AddFileAsync(string fileName, Stream data, bool overwriteExisting)
        {
            bool result = false;
            string path = Path.Combine(galleryFileRoot, fileName);
            FileMode mode = overwriteExisting ? FileMode.Create : FileMode.CreateNew;
            if (!overwriteExisting && File.Exists(path))
            {
                return result;
            }

            using (FileStream metadataStream = new FileStream(path, mode, FileAccess.Write, FileShare.None))
            {
                await data.CopyToAsync(metadataStream);
                result = true;
            }

            return result;
        }

        public void UpdateImageMetadata(Image image)
        {
            if (image.ImageMetadata != null)
            {
                SaveImageMetadata(galleryFileRoot, image);
            }
        }

        public void UpdateGalleryMetadata(string galleryCaption, string galleryLocation, DateTime? galleryDate)
        {
            this.Metadata.GalleryCaption = galleryCaption;
            this.Metadata.GalleryLocation = galleryLocation;
            this.Metadata.GalleryDate = galleryDate;
            SaveGalleryMetadata();
        }

        private void LoadMetadata(string name)
        {
            var fileName = Path.Combine(galleryFileRoot, MetadataFileName);
            if (File.Exists(fileName))
            {
                using (FileStream metadataStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (StreamReader reader = new StreamReader(metadataStream))
                    {
                        JsonTextReader jtr = new JsonTextReader(reader);
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        JsonSerializer ser = JsonSerializer.Create(settings);
                        this.galleryMetadata = ser.Deserialize<GalleryMetadata>(jtr);
                    }
                }
            }
            else
            {
                var now = DateTime.Now;

                this.galleryMetadata = new GalleryMetadata()
                {
                    Created = now,
                    LastUpdate = now,
                    Name = name
                };

                using (FileStream metadataStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (StreamWriter writer = new StreamWriter(metadataStream))
                    {
                        JsonTextWriter jtw = new JsonTextWriter(writer);
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        JsonSerializer ser = JsonSerializer.Create(settings);
                        ser.Serialize(jtw, this.galleryMetadata, typeof(GalleryMetadata));
                    }
                }
            }
        }

        private void SaveGalleryMetadata()
        {
            var fileName = Path.Combine(galleryFileRoot, MetadataFileName);
            using (FileStream metadataStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(metadataStream))
                {
                    JsonTextWriter jtw = new JsonTextWriter(writer);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    JsonSerializer ser = JsonSerializer.Create(settings);
                    ser.Serialize(jtw, this.galleryMetadata, typeof(GalleryMetadata));
                }
            }
        }

        private string RemoveSuffix(string name, string suffix)
        {
            int sufxIdx = suffix.IndexOf(".");
            int idx = name.IndexOf(suffix.Substring(1, sufxIdx-1));
            if (idx >= 0)
            {
                return name.Substring(0, idx);
            }
            return name;
        }
        private void LoadImages(string galleryWebRoot, string galleryName)
        {
            Dictionary<string, Image> imageDict = new Dictionary<string, Image>();
            
            var files = Directory.EnumerateFiles(galleryFileRoot, ThumbFileSuffix, SearchOption.TopDirectoryOnly);
            foreach(var file in files)
            {
                var fileName = RemoveSuffix(Path.GetFileName(file), ThumbFileSuffix);
                string url = string.Format("/{0}/{1}/{2}", galleryWebRoot, this.galleryMetadata.Name, Path.GetFileName(file));
                if (!imageDict.ContainsKey(fileName))
                {
                    imageDict.Add(fileName, new Image { Name = fileName, ThumbnailUrl = url });
                }
                else
                {
                    imageDict[fileName].ThumbnailUrl = url;
                }
            }

            files = Directory.EnumerateFiles(galleryFileRoot, MobileFileSuffix, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileName = RemoveSuffix(Path.GetFileName(file), MobileFileSuffix);
                string url = string.Format("/{0}/{1}/{2}", galleryWebRoot, this.galleryMetadata.Name, Path.GetFileName(file));
                if (!imageDict.ContainsKey(fileName))
                {
                    imageDict.Add(fileName, new Image { Name = fileName, MobileUrl = url });
                }
                else
                {
                    imageDict[fileName].MobileUrl = url;
                }
            }

            files = Directory.EnumerateFiles(galleryFileRoot, WebFileSuffix, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileName = RemoveSuffix(Path.GetFileName(file), WebFileSuffix);
                string url = string.Format("/{0}/{1}/{2}", galleryWebRoot, this.galleryMetadata.Name, Path.GetFileName(file));
                if (!imageDict.ContainsKey(fileName))
                {
                    imageDict.Add(fileName, new Image { Name = fileName, WebUrl = url });
                }
                else
                {
                    imageDict[fileName].WebUrl = url;
                }
            }

            files = Directory.EnumerateFiles(galleryFileRoot, FullFileSuffix, SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var fileName = RemoveSuffix(Path.GetFileName(file), FullFileSuffix);
                string url = string.Format("/{0}/{1}/{2}", galleryWebRoot, this.galleryMetadata.Name, Path.GetFileName(file));
                if (!imageDict.ContainsKey(fileName))
                {
                    imageDict.Add(fileName, new Image { Name = fileName, FullUrl = url });
                }
                else
                {
                    imageDict[fileName].FullUrl = url;
                }
            }

            List<Image> imgList = new List<Image>();
            foreach(var pair in imageDict)
            {
                imgList.Add(pair.Value);
            }

            foreach(var image in imgList)
            {
                LoadImageMetadata(galleryFileRoot, image);
            }

            this.Images = imgList;
        }

        private void SaveImageMetadata(string galleryFileRoot, Image image)
        {
            string fileName = Path.Combine(galleryFileRoot, image.Name + ImageMetadataSuffix);
            using (FileStream metadataStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(metadataStream))
                {
                    JsonTextWriter jtw = new JsonTextWriter(writer);
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    JsonSerializer ser = JsonSerializer.Create(settings);
                    ser.Serialize(jtw, image.ImageMetadata, typeof(ImageMetadata));
                }
            }
        }

        private void LoadImageMetadata(string galleryFileRoot, Image image)
        {
            var fileName = Path.Combine(galleryFileRoot, image.Name + ImageMetadataSuffix);
            if (File.Exists(fileName))
            {
                using (FileStream metadataStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    using (StreamReader reader = new StreamReader(metadataStream))
                    {
                        JsonTextReader jtr = new JsonTextReader(reader);
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        JsonSerializer ser = JsonSerializer.Create(settings);
                        image.ImageMetadata = ser.Deserialize<ImageMetadata>(jtr);
                    }
                }
            }
            else
            {
                image.ImageMetadata = new ImageMetadata();
                SaveImageMetadata(galleryFileRoot, image);
            }
        }
    }

}