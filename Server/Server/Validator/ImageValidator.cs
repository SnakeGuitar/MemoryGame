using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


namespace Server.Validator
{
    public static class ImageValidator
    {
        public const int MAX_IMAGE_SIZE_BYTES = 5 * 1024 * 1024; // 5 MB
        public static bool IsValidImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0 || imageData.Length > MAX_IMAGE_SIZE_BYTES)
            {
                return false;
            }

            if (imageData.Length > 4)
            {
                bool isJpg = imageData[0] == 0xFF && imageData[1] == 0xD8; 
                bool isPng = imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47;
                bool isBmp = imageData[0] == 0x42 && imageData[1] == 0x4D;

                if (!isJpg && !isPng && !isBmp)
                {
                    return false;
                }
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
