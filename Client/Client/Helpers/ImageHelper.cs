using Client.Properties.Langs;
using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Helpers
{
    public static class ImageHelper
    {
        private const int MAX_IMAGE_SIZE = 5 * 1024 * 1024; // 5 MB
        
        public static BitmapImage ByteArrayToImageSource(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }

            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new System.IO.MemoryStream(imageData);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            image.Freeze();
            return image;
        }

        public static byte[] ImageSourceToByteArray(BitmapImage image)
        {
            if (image == null) return Array.Empty<byte>();

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        public static byte[] ResizeImage(byte[] imageData, int maxWidth, int maxHeight)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return Array.Empty<byte>();
            }

            if (imageData.Length > MAX_IMAGE_SIZE)
            {
                throw new InvalidOperationException("Image_Too_Large");
            }

            var originalImage = ByteArrayToImageSource(imageData);
            if (originalImage == null) return Array.Empty<byte>();

            if (originalImage.PixelWidth > 5000 || originalImage.PixelHeight > 5000)
            {
                throw new InvalidOperationException("Image_Dimensions_Too_Big");
            }

            double ratioX = (double)maxWidth / originalImage.PixelWidth;
            double ratioY = (double)maxHeight / originalImage.PixelHeight;
            double ratio = Math.Min(ratioX, ratioY);

            if (ratio >= 1) return imageData;

            var resized = new TransformedBitmap(originalImage,
                new System.Windows.Media.ScaleTransform(ratio, ratio));

            var encoder = new JpegBitmapEncoder { QualityLevel = 90 };
            encoder.Frames.Add(BitmapFrame.Create(resized));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
