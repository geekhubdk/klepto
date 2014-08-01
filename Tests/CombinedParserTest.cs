using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Klepto;
using Klepto.EventProviders;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Tests
{
    [TestFixture]
    public class CombinedParserTest
    {
        [Test]
        [UseReporter(typeof(DiffReporter))]
        public void Test()
        {
            dynamic secrets = JsonConvert.DeserializeObject(File.ReadAllText("secrets.json"));
            string clientID = secrets.facebookClientID;
            string secret = secrets.facebookClientSecret;

            var instance = Kleptomanic.RegisterSchema<EventInformation>()
                .AddProvider(new TriforkProvider())
                .AddProvider(new ProsaProvider())
                .AddProvider(new MeetupProvider())
                .AddProvider(new FacebookProvider(clientID, secret))
                .AddProvider(new IdaProvider());


            var results = new List<string>();

            var urls = new[] {
                "https://www.facebook.com/events/723232537750439/?ref_dashboard_filter=upcoming",
                "http://www.meetup.com/Odense-NET-User-Group/events/190689222/",
                "https://www.prosa.dk/kalender/hele-kalenderen/?tx_moccrmintegration_courses%5Bcourse%5D=168&tx_moccrmintegration_courses%5Baction%5D=show&tx_moccrmintegration_courses%5Bcontroller%5D=Course&cHash=30e72f55030c7255d5f622191893fe99",
                "https://secure.trifork.com/aarhus-2014/freeevent/index.jsp?eventOID=6344",
                "https://www.facebook.com/events/1444628309154493/?ref_dashboard_filter=upcoming",
                 "https://secure.trifork.com/aarhus-2014/freeevent/index.jsp?eventOID=6338",
                "https://secure.trifork.com/cph-2014/freeevent/index.jsp?eventOID=6452",
                "https://secure.trifork.com/aarhus-2014/freeevent/index.jsp?eventOID=6453",
                "https://secure.trifork.com/cph-2014/freeevent/index.jsp?eventOID=6339",
                "https://secure.trifork.com/cph-2014/freeevent/index.jsp?eventOID=6461",
                "https://secure.trifork.com/cph-2014/freeevent/index.jsp?eventOID=6343",
                "https://www.prosa.dk/kalender/hele-kalenderen/?tx_moccrmintegration_courses%5Bcourse%5D=278&tx_moccrmintegration_courses%5Baction%5D=show&tx_moccrmintegration_courses%5Bcontroller%5D=Course&cHash=58bb73d2116161676953f6edb99f269c",
                "http://ida.dk/event/311253",
                "http://ida.dk/event/310836"
            };

            
            foreach (string url in urls) {
                var data = new StringBuilder();
                try {
                    data.AppendLine(url);
                    var result = instance.GetResult(url);
                    if (result != null) {
                        data.AppendLine(result.Title);
                        data.AppendLine(result.Date != null ? result.Date.Value.ToString("dd/MM/yyyy") : "Ukendt");
                        data.AppendLine(result.Organizer);
                        data.AppendLine(result.City);
                    } else {
                        data.Append("Failed");
                    }
                }
                catch (Exception ex) {
                    data.Append(ex.Message);
                }

                results.Add(data.ToString());
            }

            Approvals.Verify(string.Join(Environment.NewLine, results));
        }

    }
}
