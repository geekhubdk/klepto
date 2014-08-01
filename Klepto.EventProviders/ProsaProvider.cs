using System;
using System.Globalization;
using System.Linq;

namespace Klepto.EventProviders
{
    public class ProsaProvider : ISchemaProvider<EventInformation>
    {
        public EventInformation GetResult(string url)
        {
            var uri = new Uri(url);
            if (uri.Authority != "www.prosa.dk")
                return null;

            var schema = new EventInformation();
            var helper = new HtmlSchemaHelper<EventInformation>();
            helper.Load(url);

            helper.Fetch(schema, ".properties h2", x => x.Title);
            schema.Organizer = "Prosa";
            helper.Fetch(schema, ".inner .details:nth-child(1) .big", (x, y) => x.Date = FuzzyParseDate(y));
            helper.Fetch(schema, ".inner .details:nth-child(3) p", (x, y) => x.City = FuzzyParseCity(y));

            return schema;
        }

        private string FuzzyParseCity(string s)
        {
            var tokens = s.Split(',');
            var last = tokens.Last();
            return last.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        private DateTimeOffset FuzzyParseDate(string s)
        {
            var tokens = s.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            return DateTime.Parse(tokens[0], new CultureInfo("da-DK"));
        }
    }
}
