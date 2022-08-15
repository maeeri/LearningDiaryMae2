using System;
using NUnit.Framework;
using LearningDiaryMae2;
using LearningDiaryMae2.Models;
using LearningDiaryMae2.Attributes;

namespace MethodsTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DateCheckTestFuture()
        {
            var result = Methods.DateCheck(new DateTime(2022, 12, 12), new DateTime(2022, 10, 12));
            Assert.AreEqual(false, result);
        }

        [Test]
        public void DateCheckTestPast()
        {
            var result = Methods.DateCheck(new DateTime(2022, 10, 12), new DateTime(2022, 12, 12));
            Assert.AreEqual(true, result);
        }

        [Test]
        public void DateCheckTestSame()
        {
            var result = Methods.DateCheck(new DateTime(2022, 12, 12), new DateTime(2022, 12, 12));
            Assert.AreEqual(false, result);
        }
    }
}