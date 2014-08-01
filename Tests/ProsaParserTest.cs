using System;
using Klepto;
using Klepto.EventProviders;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ProsaParserTest
    {
        private EventInformation _result;

        public ProsaParserTest()
        {
            var instance = Kleptomanic.RegisterSchema<EventInformation>().
                AddProvider(new ProsaProvider());

            _result = instance.GetResult("https://www.prosa.dk/kalender/hele-kalenderen/?tx_moccrmintegration_courses%5Bcourse%5D=168&tx_moccrmintegration_courses%5Baction%5D=show&tx_moccrmintegration_courses%5Bcontroller%5D=Course&cHash=30e72f55030c7255d5f622191893fe99");

        }

        [Test]
        public void Title()
        {
            Assert.AreEqual("Geek Tuesday - Big Data (Aarhus U35)", _result.Title);
        }

        [Test]
        public void Date()
        {
            Assert.AreEqual(DateTimeOffset.Parse("2014/08/19"), _result.Date);
        }

        [Test]
        public void Organizer()
        {
            Assert.AreEqual("Prosa", _result.Organizer);
        }

        [Test]
        public void City()
        {
            Assert.AreEqual("Aarhus", _result.City);
        }
    }
}
