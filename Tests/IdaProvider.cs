using System;
using Klepto;
using Klepto.EventProviders;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class IdaParserTest
    {
        private EventInformation _result;

        public IdaParserTest()
        {
            var instance = Kleptomanic.RegisterSchema<EventInformation>().
                AddProvider(new IdaProvider());

            _result = instance.GetResult("http://ida.dk/event/311253");

        }

        [Test]
        public void Title()
        {
            Assert.AreEqual("AngularJS 100% JavaScript", _result.Title);
        }

        [Test]
        public void Date()
        {
            Assert.AreEqual(DateTimeOffset.Parse("2014/09/11"), _result.Date);
        }

        [Test]
        public void Organizer()
        {
            Assert.AreEqual("IDA IT", _result.Organizer);
        }

        [Test]
        public void City()
        {
            Assert.AreEqual("København", _result.City);
        }
    }
}
