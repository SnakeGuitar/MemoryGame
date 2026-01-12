using Client.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using Assert = NUnit.Framework.Assert;

namespace Client.Tests.Helpers
{
    [TestFixture]
    public class ClientHelperTests
    {
        [Test]
        public void GenerateGameCode_ReturnsStringWithLengthSix()
        {
            string code = ClientHelper.GenerateGameCode();
            Assert.That(code.Length, Is.EqualTo(6));
        }

        [Test]
        public void GenerateGameCode_ReturnsNumericString()
        {
            string code = ClientHelper.GenerateGameCode();
            Assert.That(int.TryParse(code, out _), Is.True, "Result must be numeric.");
        }

        [Test]
        public void GenerateGameCode_ReturnsValueWithinRange()
        {
            // Logic: guarantees range [100000, 999999]
            int codeValue = int.Parse(ClientHelper.GenerateGameCode());

            // NUnit Specific assertion for range
            Assert.That(codeValue, Is.InRange(100000, 999999));
        }

        [Test]
        public void GenerateGameCode_IsRandom()
        {
            // Verify that calling it twice doesn't return the exact same value immediately
            string code1 = ClientHelper.GenerateGameCode();
            string code2 = ClientHelper.GenerateGameCode();

            Assert.That(code1, Is.Not.EqualTo(code2), "Two consecutive calls should produce different codes.");
        }

        [Test]
        public void GenerateGameCode_MultipleIterations_AlwaysValid()
        {
            // Run loop to ensure stability across multiple random generations
            Assert.DoesNotThrow(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    string code = ClientHelper.GenerateGameCode();
                    int val = int.Parse(code);

                    if (code.Length != 6 || val < 100000 || val > 999999)
                    {
                        throw new System.Exception($"Generated invalid code: {code}");
                    }
                }
            });
        }
    }
}