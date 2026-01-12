using Client.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using Assert = NUnit.Framework.Assert;

namespace Client.Test.Models
{
    [TestFixture]
    public class DifficultyPresetsTests
    {

        [Test]
        public void CalculateLayout_40Cards_Returns5Rows8Columns()
        {
            var result = DifficultyPresets.CalculateLayout(40);
            Assert.That(result.Rows, Is.EqualTo(5));
            Assert.That(result.Columns, Is.EqualTo(8));
        }

        [Test]
        public void CalculateLayout_30Cards_Returns5Rows6Columns()
        {
            var result = DifficultyPresets.CalculateLayout(30);
            Assert.That(result.Rows, Is.EqualTo(5));
            Assert.That(result.Columns, Is.EqualTo(6));
        }

        [Test]
        public void CalculateLayout_24Cards_Returns4Rows6Columns()
        {
            var result = DifficultyPresets.CalculateLayout(24);
            Assert.That(result.Rows, Is.EqualTo(4));
            Assert.That(result.Columns, Is.EqualTo(6));
        }

        [Test]
        public void CalculateLayout_16Cards_Returns4Rows4Columns()
        {
            var result = DifficultyPresets.CalculateLayout(16);
            Assert.That(result.Rows, Is.EqualTo(4));
            Assert.That(result.Columns, Is.EqualTo(4));
        }

        [Test]
        public void CalculateLayout_DefaultUnknownValue_Returns4Rows4Columns()
        {
            var result = DifficultyPresets.CalculateLayout(999);
            Assert.That(result.Rows, Is.EqualTo(4));
            Assert.That(result.Columns, Is.EqualTo(4));
        }

        [Test]
        public void Easy_Preset_HasCorrectValues()
        {
            var config = DifficultyPresets.Easy;

            Assert.That(config.NumberOfCards, Is.EqualTo(16));
            Assert.That(config.TimeLimitSeconds, Is.EqualTo(60));
            Assert.That(config.NumberRows, Is.EqualTo(4));
            Assert.That(config.NumberColumns, Is.EqualTo(4));
            Assert.That(config.DifficultyLevel, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Normal_Preset_HasCorrectValues()
        {
            var config = DifficultyPresets.Normal;

            Assert.That(config.NumberOfCards, Is.EqualTo(24));
            Assert.That(config.TimeLimitSeconds, Is.EqualTo(90));
            Assert.That(config.NumberRows, Is.EqualTo(4));
            Assert.That(config.NumberColumns, Is.EqualTo(6));
            Assert.That(config.DifficultyLevel, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void Hard_Preset_HasCorrectValues()
        {
            var config = DifficultyPresets.Hard;

            Assert.That(config.NumberOfCards, Is.EqualTo(30));
            Assert.That(config.TimeLimitSeconds, Is.EqualTo(120));
            Assert.That(config.NumberRows, Is.EqualTo(5));
            Assert.That(config.NumberColumns, Is.EqualTo(6));
            Assert.That(config.DifficultyLevel, Is.Not.Null.And.Not.Empty);
        }
    }

    [TestFixture]
    public class GameConfigurationTests
    {

        [Test]
        public void Constructor_SetsPropertiesCorrectly()
        {
            int cards = 10;
            int seconds = 100;
            int rows = 2;
            int cols = 5;
            string diff = "Extreme";

            var config = new GameConfiguration(cards, seconds, rows, cols, diff);

            Assert.That(config.NumberOfCards, Is.EqualTo(cards));
            Assert.That(config.TimeLimitSeconds, Is.EqualTo(seconds));
            Assert.That(config.NumberRows, Is.EqualTo(rows));
            Assert.That(config.NumberColumns, Is.EqualTo(cols));
            Assert.That(config.DifficultyLevel, Is.EqualTo(diff));
        }
    }
} 
