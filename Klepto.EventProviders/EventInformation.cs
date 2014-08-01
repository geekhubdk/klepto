using System;

namespace Klepto.EventProviders
{
    public class EventInformation
    {
        public string Title { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string Organizer { get; set; }
        public string City { get; set; }
    }
}