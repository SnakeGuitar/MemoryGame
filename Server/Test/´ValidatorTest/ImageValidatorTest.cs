using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Shared;
using Server.Validator;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Test
{
    [TestClass]
    public class ImageValidatorTest
    {
        /// <summary>
        /// Creates a valid image in memory and returns its byte array.
        /// This avoids dependency on physical files.
        /// </summary>
        private byte[] GenerateMockImageBytes(int width, int height, ImageFormat format)
        {
            using (var bitmap = new Bitmap(width, height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.Blue);
                }
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, format);
                    return stream.ToArray();
                }
            }
        }

        [TestMethod]
        public void IsValidImage_NullData_ReturnsFalse()
        {
            bool result = ImageValidator.IsValidImage(null);

            Assert.IsFalse(result, "Should return false for null input.");
        }

        [TestMethod]
        public void IsValidImage_EmptyData_ReturnsFalse()
        {
            bool result = ImageValidator.IsValidImage(new byte[0]);

            Assert.IsFalse(result, "Should return false for empty byte array.");
        }

        [TestMethod]
        public void IsValidImage_SizeExceedsLimit_ReturnsFalse()
        {
            int tooBigSize = (5 * 1024 * 1024) + 1;
            byte[] bigData = new byte[tooBigSize];

            bool result = ImageValidator.IsValidImage(bigData);

            Assert.IsFalse(result, "Should return false if file size is larger than 5MB.");
        }

        [TestMethod]
        public void IsValidImage_GarbageBytes_ReturnsFalse()
        {
            byte[] garbageData = new byte[] { 0xFF, 0x00, 0xAA, 0xBB, 0x12, 0x34 };

            bool result = ImageValidator.IsValidImage(garbageData);

            Assert.IsFalse(result, "Should return false (and handle exception internally) for corrupted/invalid image data.");
        }

        [TestMethod]
        public void IsValidImage_ValidJpeg_ReturnsTrue()
        {
            byte[] jpegBytes = GenerateMockImageBytes(100, 100, ImageFormat.Jpeg);

            bool result = ImageValidator.IsValidImage(jpegBytes);

            Assert.IsTrue(result, "Should accept valid JPEG images.");
        }

        [TestMethod]
        public void IsValidImage_ValidPng_ReturnsTrue()
        {
            byte[] pngBytes = GenerateMockImageBytes(100, 100, ImageFormat.Png);

            bool result = ImageValidator.IsValidImage(pngBytes);

            Assert.IsTrue(result, "Should accept valid PNG images.");
        }

        [TestMethod]
        public void IsValidImage_ValidBmp_ReturnsTrue()
        {
            byte[] bmpBytes = GenerateMockImageBytes(50, 50, ImageFormat.Bmp);

            bool result = ImageValidator.IsValidImage(bmpBytes);

            Assert.IsTrue(result, "Should accept valid BMP images.");
        }

        [TestMethod]
        public void IsValidImage_InvalidFormatGif_ReturnsFalse()
        {
            byte[] gifBytes = GenerateMockImageBytes(50, 50, ImageFormat.Gif);

            bool result = ImageValidator.IsValidImage(gifBytes);

            Assert.IsFalse(result, "Should reject GIF images (only Jpeg, Png, Bmp allowed).");
        }

        [TestMethod]
        public void IsValidImage_InvalidFormatTiff_ReturnsFalse()
        {
            byte[] tiffBytes = GenerateMockImageBytes(50, 50, ImageFormat.Tiff);

            bool result = ImageValidator.IsValidImage(tiffBytes);

            Assert.IsFalse(result, "Should reject TIFF images.");
        }

        [TestMethod]
        public void IsValidImage_DimensionsTooWide_ReturnsFalse()
        {
            byte[] wideImage = GenerateMockImageBytes(4097, 10, ImageFormat.Png);

            bool result = ImageValidator.IsValidImage(wideImage);

            Assert.IsFalse(result, "Should return false if Width > 4096.");
        }

        [TestMethod]
        public void IsValidImage_DimensionsTooTall_ReturnsFalse()
        {
            byte[] tallImage = GenerateMockImageBytes(10, 4097, ImageFormat.Jpeg);

            bool result = ImageValidator.IsValidImage(tallImage);

            Assert.IsFalse(result, "Should return false if Height > 4096.");
        }

        [TestMethod]
        public void IsValidImage_MaxAllowedDimensions_ReturnsTrue()
        {
            byte[] boundaryImage = GenerateMockImageBytes(4096, 100, ImageFormat.Png);

            bool result = ImageValidator.IsValidImage(boundaryImage);

            Assert.IsTrue(result, "Should return true for width exactly 4096.");
        }
    }
}