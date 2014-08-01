using System;
using System.Globalization;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace Klepto.EventProviders
{
    public class FacebookProvider : ISchemaProvider<EventInformation>
    {
        private readonly string _clientID;
        private readonly string _clientSecret;

        public FacebookProvider(string clientID, string clientSecret)
        {
            _clientID = clientID;
            _clientSecret = clientSecret;
        }

        public EventInformation GetResult(string url)
        {
            var uri = new Uri(url);

            if (uri.Authority != "facebook.com" &&
                uri.Authority != "www.facebook.com" &&
                uri.Authority != "fb.me")
                return null;

            var schema = new EventInformation();
            var json = new WebClient().DownloadString(string.Format("https://graph.facebook.com?ids={0}&access_token={1}|{2}", HttpUtility.UrlEncode(url), _clientID, _clientSecret));

            dynamic obj = JsonConvert.DeserializeObject(json);
            schema.Title = obj[url].name;
            string date = obj[url].start_time;
            schema.Date = DateTimeOffset.ParseExact(date, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en-us")).Date;
            schema.City = obj[url].venue.city;

            return schema;
        }
    }
}
