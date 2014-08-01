using System;
using Klepto;
using Klepto.EventProviders;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MeetupParserTest
    {
        private EventInformation _result;

        public MeetupParserTest()
        {
            var instance = Kleptomanic.RegisterSchema<EventInformation>().
                AddProvider(new MeetupProvider());

            _result = instance.GetResult("http://www.meetup.com/Odense-NET-User-Group/events/190689222/");
            
        }

        [Test]
        public void Title()
        {
            Assert.AreEqual("Arkitektur - Open Spaces", _result.Title);
        }

        [Test]
        public void Date()
        {
            Assert.AreEqual(DateTimeOffset.Parse("2014/08/05"), _result.Date);
        }

        [Test]
        public void Organizer()
        {
            Assert.AreEqual("Odense .NET User Group", _result.Organizer);
        }

        [Test]
        public void City()
        {
            Assert.AreEqual("Odense", _result.City);
        }
    }
}
