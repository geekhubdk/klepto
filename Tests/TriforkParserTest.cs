using System;
using Klepto;
using Klepto.EventProviders;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class TriforkParserTest
    {
        private EventInformation _result;

        public TriforkParserTest()
        {
            var instance = Kleptomanic.RegisterSchema<EventInformation>().
                AddProvider(new TriforkProvider());

            _result = instance.GetResult("https://secure.trifork.com/aarhus-2014/freeevent/index.jsp?eventOID=6344");

        }

        [Test]
        public void Title()
        {
            Assert.AreEqual("Distributed systems using ZeroMq", _result.Title);
        }

        [Test]
        public void Date()
        {
            Assert.AreEqual(DateTimeOffset.Parse("2014/08/12"), _result.Date);
        }

        [Test]
        public void Organizer()
        {
            Assert.AreEqual("Trifork", _result.Organizer);
        }

        [Test]
        public void City()
        {
            Assert.AreEqual("Aarhus", _result.City);
        }
    }
}
