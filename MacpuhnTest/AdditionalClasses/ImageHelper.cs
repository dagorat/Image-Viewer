using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace MacpuhnTest.AdditionalClasses
{
    internal static class ImageHelper
    {

        private static readonly List<string> ValidExtensions = new List<string>{ ".jpg", ".bmp", ".gif", ".png", ".tif", ".ico" }; //  etc

        public static bool IsImageExtension(string ext)
        {
            return ValidExtensions.Contains(ext);
        }

        public static BitmapImage LoadImageFromFile(string filename, int decodedWidth)
        {
            using (var fs = File.OpenRead(filename))
            {
                var img = new BitmapImage();
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.DecodePixelWidth = decodedWidth;
                img.StreamSource = fs;
                img.EndInit();
                img.Freeze();
                return img;
            }
        }

    }
}
