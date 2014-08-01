using System;
using System.IO;
using Klepto;
using Klepto.EventProviders;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class FacebookProviderTest
    {
        private EventInformation _result;

        public FacebookProviderTest()
        {
            dynamic secrets = JsonConvert.DeserializeObject(File.ReadAllText("secrets.json"));
            string clientID = secrets.facebookClientID;
            string secret = secrets.facebookClientSecret;
            var instance = Kleptomanic.RegisterSchema<EventInformation>().
                AddProvider(new FacebookProvider(clientID, secret));

            _result = instance.GetResult("https://www.facebook.com/events/723232537750439/?ref_dashboard_filter=upcoming");
        }

        [Test]
        public void Title()
        {
            Assert.AreEqual("Event sourcing og CQRS", _result.Title);
        }

        [Test]
        public void Date()
        {
            Assert.AreEqual(DateTimeOffset.Parse("2014/08/27"), _result.Date);
        }

        [Test]
        public void Organizer()
        {
            Assert.AreEqual(null, _result.Organizer);
        }

        [Test]
        public void City()
        {
            Assert.AreEqual("Åbyhøj", _result.City);
        }
    }
}
