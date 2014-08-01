using System;

namespace Klepto.EventProviders
{
    public class IdaProvider : ISchemaProvider<EventInformation>
    {
        public EventInformation GetResult(string url)
        {
            var uri = new Uri(url);
            if (uri.Authority != "ida.dk")
                return null;

            var schema = new EventInformation();
            var helper = new HtmlSchemaHelper<EventInformation>();
            helper.Load(url);

            helper.Fetch(schema, "h1", x => x.Title);
            helper.Fetch(schema, ".field-name-field-coordinator", x => x.Organizer);
            helper.Fetch(schema, ".date-display-single", (x, y) => x.Date = FussyParseDate(y));
            helper.Fetch(schema, ".pane-node-field-event-location p div:last-child", (x, y) => x.City = FussyParseLocation(y));

            return schema;
        }

        private string FussyParseLocation(string s)
        {
            return s.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        private DateTimeOffset FussyParseDate(string s)
        {
            var tokens = s.Split(new[] { "-", "," }, StringSplitOptions.RemoveEmptyEntries);
            return DateTimeOffset.Parse(tokens[1].Trim());
        }
    }
}
