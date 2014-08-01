using System;

namespace Klepto.EventProviders
{
    public class MeetupProvider : ISchemaProvider<EventInformation>
    {
        public EventInformation GetResult(string url)
        {
            var uri = new Uri(url);
            if (uri.Authority != "meetup.com" &&
                uri.Authority != "www.meetup.com")
                return null;

            var schema = new EventInformation();
            var helper = new HtmlSchemaHelper<EventInformation>();
            helper.Load(url);

            helper.Fetch(schema, "#event-title h1", x => x.Title);
            helper.Fetch(schema, "#chapter-banner h1", x => x.Organizer);
            helper.Fetch(schema, "#event-start-time h3", (x, y) => x.Date = DateTime.Parse(y));
            helper.Fetch(schema, ".locality", x => x.City);

            return schema;
        }
    }
}
