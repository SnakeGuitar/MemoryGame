using System;
using System.Drawing;
using System.IO;

namespace Server.Validator
{
    public static class ImageValidator
    {
        public const int MAX_IMAGE_SIZE_BYTES = 5 * 1024 * 1024;
        public const int MAX_DIMENSION = 4096;

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
                using (var ms = new MemoryStream(imageData))
                {
                    using (var img = System.Drawing.Image.FromStream(ms))
                    {
                        if (img.Width > MAX_DIMENSION || img.Height > MAX_DIMENSION)
                        {
                            return false;
                        }

                        return img.Width > 0 && img.Height > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
