using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klepto.EventProviders
{
    public class TriforkProvider : ISchemaProvider<EventInformation>
    {
        public EventInformation GetResult(string url)
        {
            var uri = new Uri(url);
            if (uri.Authority != "secure.trifork.com")
                return null;

            var schema = new EventInformation();
            var helper = new HtmlSchemaHelper<EventInformation>();
            helper.Load(url);

            helper.Fetch(schema, ".text h1", x => x.Title);
            schema.Organizer = "Trifork";
            helper.Fetch(schema, ".text p:nth-child(3)", (x, y) => x.Date = FuzzyParseDate(y));
            helper.Fetch(schema, ".text p:nth-child(4)", (x, y) => x.City = FuzzyParseCity(y));
            
            return schema;
        }

        private string FuzzyParseCity(string s)
            {
                var tokens = s.Split(',');
                var last = tokens.Last();
                return last.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries)[1];
        }

        private DateTimeOffset FuzzyParseDate(string s)
        {
            var output = s.Replace("When:","");
            output = output.Substring(0, output.IndexOf("at", StringComparison.CurrentCultureIgnoreCase));
            output = output.Trim();
            return DateTime.Parse(output);
        }
    }
}
