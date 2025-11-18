using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Client.Helpers
{
    public static class ImageHelper
    {
        private const int MAX_IMAGE_SIZE = 5 * 1024 * 1024; // 5 MB
        // FOR WPF APPLICATIONS
        public static BitmapImage ByteArrayToImageSource(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
            {
                return null;
            }
            try
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = new System.IO.MemoryStream(imageData);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze(); // Freeze for cross-thread operations
                return image;
            }
            catch
            {
                return null;
            }
        }

        // FOR SERVER
        public static byte[] ImageSourceToByteArray(BitmapImage image)
        {
            if (image == null)
            {
                return null;
            }

            try
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var stream = new System.IO.MemoryStream())
                {
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        public static byte[] ResizeImage(byte[] imageData, int maxWidth, int maxHeight)
        {
            if (imageData == null || imageData.Length == 0 || imageData.Length > MAX_IMAGE_SIZE)
            {
                return null;
            }

            try
            {
                var originalImage = ByteArrayToImageSource(imageData);
                if (originalImage == null)
                {
                    return imageData;
                }

                if (originalImage.PixelWidth > 10000 || originalImage.PixelHeight > 10000)
                {
                    return null; // Image too large to process
                }

                double ratioX = (double)maxWidth / originalImage.PixelWidth;
                double ratioY = (double)maxHeight / originalImage.PixelHeight;
                double ratio = Math.Min(ratioX, ratioY);

                if (ratio >= 1)
                {
                    return imageData; // No resizing needed
                }

                var resized = new TransformedBitmap(originalImage,
                    new System.Windows.Media.ScaleTransform(ratio, ratio));

                var encoder = new JpegBitmapEncoder
                {
                    QualityLevel = 90
                };
                encoder.Frames.Add(BitmapFrame.Create(resized));

                using (var stream = new System.IO.MemoryStream())
                {
                    encoder.Save(stream);
                    return stream.ToArray();
                }
            }
            catch
            {
                return imageData;
            }
        }
    }
}
