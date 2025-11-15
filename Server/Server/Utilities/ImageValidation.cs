using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


namespace Server.Utilities
{
    public static class ImageValidation
    {
        public const int MAX_IMAGE_SIZE_BYTES = 5 * 1024 * 1024; // 5 MB
        public static bool IsValidImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0 || imageData.Length > MAX_IMAGE_SIZE_BYTES)
            {
                return false;
            }

            try
            {
                using (var stream = new System.IO.MemoryStream(imageData))
                {
                    var image = Image.FromStream(stream);
                    if (image.Width > 4096 || image.Height > 4096)
                    {
                        return false;
                    }
                    return image.RawFormat.Equals(ImageFormat.Jpeg) ||
                           image.RawFormat.Equals(ImageFormat.Png) ||
                           image.RawFormat.Equals(ImageFormat.Bmp);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
