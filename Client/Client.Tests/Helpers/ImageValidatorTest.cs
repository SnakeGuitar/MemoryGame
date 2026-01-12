using Client.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;
using System.Windows.Media.Imaging;
using Assert = NUnit.Framework.Assert;

namespace Client.Tests.Helpers
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ImageHelperTests
    {
        // Limit defined in ImageHelper
        private const int MAX_IMAGE_SIZE_BYTES = 5 * 1024 * 1024;

        /// <summary>
        /// Helper method to generate a valid WPF-compatible byte array representing an image.
        /// </summary>
        private byte[] GenerateMockImageBytes(int width, int height)
        {
            var pixelFormat = System.Windows.Media.PixelFormats.Bgra32;
            int stride = (width * pixelFormat.BitsPerPixel + 7) / 8;
            byte[] pixels = new byte[stride * height];

            var bitmapSource = BitmapSource.Create(width, height, 96, 96, pixelFormat, null, pixels, stride);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        #region ByteArrayToImageSource Tests

        [Test]
        public void ByteArrayToImageSource_NullData_ReturnsNull()
        {
            var result = ImageHelper.ByteArrayToImageSource(null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ByteArrayToImageSource_EmptyData_ReturnsNull()
        {
            var result = ImageHelper.ByteArrayToImageSource(Array.Empty<byte>());
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ByteArrayToImageSource_ValidData_ReturnsNotNull()
        {
            byte[] validData = GenerateMockImageBytes(10, 10);
            var result = ImageHelper.ByteArrayToImageSource(validData);
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ByteArrayToImageSource_ValidData_ReturnsFrozenImage()
        {
            byte[] validData = GenerateMockImageBytes(10, 10);
            var result = ImageHelper.ByteArrayToImageSource(validData);
            Assert.That(result.IsFrozen, Is.True, "The BitmapImage must be frozen.");
        }

        [Test]
        public void ByteArrayToImageSource_ValidData_ReturnsCorrectWidth()
        {
            byte[] validData = GenerateMockImageBytes(10, 10);
            var result = ImageHelper.ByteArrayToImageSource(validData);
            Assert.That(result.PixelWidth, Is.EqualTo(10));
        }

        #endregion

        #region ResizeImage Tests

        [Test]
        public void ResizeImage_SizeExceedsLimit_ThrowsInvalidOperationException()
        {
            byte[] heavyData = new byte[MAX_IMAGE_SIZE_BYTES + 1];

            Assert.Throws<InvalidOperationException>(() =>
            {
                ImageHelper.ResizeImage(heavyData, 100, 100);
            });
        }

        [Test]
        public void ResizeImage_SizeExceedsLimit_ThrowsCorrectMessage()
        {
            byte[] heavyData = new byte[MAX_IMAGE_SIZE_BYTES + 1];

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                ImageHelper.ResizeImage(heavyData, 100, 100);
            });

            Assert.That(ex.Message, Is.EqualTo("Image_Too_Large"));
        }

        [Test]
        public void ResizeImage_DimensionsTooBig_ThrowsInvalidOperationException()
        {
            byte[] wideData = GenerateMockImageBytes(5001, 10);

            Assert.Throws<InvalidOperationException>(() =>
            {
                ImageHelper.ResizeImage(wideData, 100, 100);
            });
        }

        [Test]
        public void ResizeImage_DimensionsTooBig_ThrowsCorrectMessage()
        {
            byte[] wideData = GenerateMockImageBytes(5001, 10);

            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                ImageHelper.ResizeImage(wideData, 100, 100);
            });

            Assert.That(ex.Message, Is.EqualTo("Image_Dimensions_Too_Big"));
        }

        [Test]
        public void ResizeImage_SmallImage_ReturnsOriginalData()
        {
            byte[] smallData = GenerateMockImageBytes(50, 50);

            var result = ImageHelper.ResizeImage(smallData, 100, 100);

            Assert.That(result, Is.SameAs(smallData));
        }

        [Test]
        public void ResizeImage_ValidResize_ReturnsNotNull()
        {
            byte[] largeData = GenerateMockImageBytes(200, 200);
            byte[] resizedData = ImageHelper.ResizeImage(largeData, 50, 50);

            Assert.That(resizedData, Is.Not.Null);
        }

        [Test]
        public void ResizeImage_ValidResize_ReturnsDifferentData()
        {
            byte[] largeData = GenerateMockImageBytes(200, 200);
            byte[] resizedData = ImageHelper.ResizeImage(largeData, 50, 50);

            Assert.That(resizedData.Length, Is.Not.EqualTo(largeData.Length));
        }

        [Test]
        public void ResizeImage_ValidResize_ReturnsSmallerDimensions()
        {
            byte[] largeData = GenerateMockImageBytes(200, 200);
            byte[] resizedData = ImageHelper.ResizeImage(largeData, 50, 50);

            var newImage = ImageHelper.ByteArrayToImageSource(resizedData);
            Assert.That(newImage.PixelWidth, Is.LessThanOrEqualTo(50));
        }

        #endregion

        #region ImageSourceToByteArray Tests

        [Test]
        public void ImageSourceToByteArray_ValidImage_ReturnsNotNull()
        {
            byte[] originalBytes = GenerateMockImageBytes(10, 10);
            var imageSource = ImageHelper.ByteArrayToImageSource(originalBytes);

            byte[] resultBytes = ImageHelper.ImageSourceToByteArray(imageSource);

            Assert.That(resultBytes, Is.Not.Null);
        }

        [Test]
        public void ImageSourceToByteArray_ValidImage_ReturnsNotEmpty()
        {
            byte[] originalBytes = GenerateMockImageBytes(10, 10);
            var imageSource = ImageHelper.ByteArrayToImageSource(originalBytes);

            byte[] resultBytes = ImageHelper.ImageSourceToByteArray(imageSource);

            Assert.That(resultBytes.Length, Is.GreaterThan(0));
        }

        #endregion
    }
}