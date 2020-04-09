using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using System;
using System.Drawing;
using System.Linq;
using Asset.Infrastructure._App;

namespace Presentation.Panel.Helpers
{
    public class ThumbnailHelper
    {
        #region Constructor

        private static ILog4Net _logger;

        static ThumbnailHelper()
        {
            _logger = ServiceLocatorAdapter.Current.GetInstance<ILog4Net>();
        }

        #endregion

        #region Private
        private static ResizeMode SetResizeMode(Size size)
        {
            if (size.Width >= size.Height)
            {
                return ResizeMode.Crop;
            }
            else
            {
                return ResizeMode.Pad;
            }
        }
        #endregion
        public static bool Init(string url, int? id = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }
            //CreateDirectories();
            var serverPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
            var absolutePath = serverPath + url;
            if (System.IO.File.Exists(absolutePath))
            {
                ResizeLayer resizeLayer = null;
                var size = new Size();
                JpegFormat jpegFormat = null;
                var imagePath = string.Empty;
                try
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        var sizeCount = AppSettings.ImageSizes.Count();
                        for (int i = 0; i < sizeCount; i++)
                        {
                            var dimentions = AppSettings.ImageSizes[i].StrValue.Split('x');
                            size = new Size(int.Parse(dimentions[0]), int.Parse(dimentions[1]));
                            resizeLayer = new ResizeLayer(size, SetResizeMode(size));
                            if (AppSettings.ImageSizes[i].KeyName.Contains("Thumb"))
                            {
                                //resizeLayer = new ResizeLayer(size, ResizeMode.Stretch);
                                jpegFormat = new JpegFormat { Quality = 95 };
                            }
                            else if (AppSettings.ImageSizes[i].KeyName.Contains("Max"))
                            {
                                //resizeLayer = new ResizeLayer(size, ResizeMode.Pad);
                                jpegFormat = new JpegFormat { Quality = 90 };
                            }
                            else
                            {
                                //resizeLayer = new ResizeLayer(size, SetResizeMode(size));
                                jpegFormat = new JpegFormat { Quality = 90 };
                            }
                            imagePath = string.Format(@"{0}Content\TransformedImages\{1}\{2}.jpg", serverPath, id, AppSettings.ImageSizes[i].StrValue);
                            imageFactory.Load(absolutePath).Resize(resizeLayer).Format(jpegFormat).Save(imagePath);
                        }
                    }
                    return true;
                }
                catch (Exception eX)
                {
                    _logger.Error(eX);
                    return false;
                }
            }
            else
            {
                _logger.Error($"{absolutePath} - Not Found!");
            }
            return false;
        }
    }
}