using NUnit.Framework;
using Client.Core.Converters;
using Client.Core;
using System;
using System.Globalization;
using System.Windows;
using Assert = NUnit.Framework.Assert;

namespace Client.Test.Core.Converters
{
    [TestFixture]
    public class SelfReportVisibilityConverterTests
    {
        private SelfReportVisibilityConverter _converter;

        [SetUp]
        public void Setup()
        {
            _converter = new SelfReportVisibilityConverter();
            UserSession.Username = null;
        }

        [TearDown]
        public void TearDown()
        {
            UserSession.Username = null;
        }

        [Test]
        public void Convert_ValueIsCurrentUser_ReturnsCollapsed()
        {
            UserSession.Username = "PlayerOne";
            string value = "PlayerOne";

            var result = _converter.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
        }

        [Test]
        public void Convert_ValueIsDifferentUser_ReturnsVisible()
        {
            UserSession.Username = "PlayerOne";
            string value = "PlayerTwo";

            var result = _converter.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
        }

        [Test]
        public void Convert_ValueIsNull_ReturnsVisible()
        {
            UserSession.Username = "PlayerOne";
            object value = null;

            var result = _converter.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
        }

        [Test]
        public void Convert_ValueIsNotString_ReturnsVisible()
        {
            UserSession.Username = "PlayerOne";
            int value = 123; // Incorrect type

            var result = _converter.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
        }

        [Test]
        public void Convert_SessionUsernameIsNull_ReturnsVisible()
        {
            UserSession.Username = null;
            string value = "PlayerOne";

            var result = _converter.Convert(value, typeof(Visibility), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
        }

        [Test]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            object value = Visibility.Visible;

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(value, typeof(string), null, CultureInfo.InvariantCulture));
        }
    }
}