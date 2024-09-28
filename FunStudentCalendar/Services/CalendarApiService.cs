using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FunStudentCalendar.Services
{
    public class CalendarApiService
    {
        private string _credentials;

        public CalendarApiService(string credentials)
        {
            _credentials = credentials;
        }

        public string GetEvents(long unixTimeStampStart, long unixTimeStampEnd, string calendarEmail)
        {
            string[] Scopes = { CalendarService.Scope.Calendar };
            GoogleCredential credential;

            credential = GoogleCredential.FromJson(_credentials).CreateScoped(Scopes).CreateWithUser(calendarEmail);

            var services = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });

            if (services == null) { return ""; }
            var calendar = services.Calendars.Get("primary");
            var eventsRequest = services.Events.List(calendar.CalendarId);
            eventsRequest.TimeMaxDateTimeOffset = UnixToDateTime(unixTimeStampEnd);
            eventsRequest.TimeMinDateTimeOffset = UnixToDateTime(unixTimeStampStart);
            var events = eventsRequest.Execute().Items;

            List<EventStore> eventList = new List<EventStore>();

            foreach (var singleEvent in events)
            {
                eventList.Add(new EventStore(singleEvent.Id, DateTimeToUnix(singleEvent.Start.DateTimeRaw), DateTimeToUnix(singleEvent.End.DateTimeRaw), singleEvent.Summary, singleEvent.Description));
            }

            string jsonString = JsonSerializer.Serialize(eventList);

            return jsonString;
        }

        private DateTime UnixToDateTime(long unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        private long? DateTimeToUnix(string dateTime)
        {
            if (dateTime == "") { return null; }
            DateTime newDateTime = DateTime.Parse(dateTime);
            return new DateTimeOffset(newDateTime).ToUnixTimeSeconds();
        }
    }

    public class EventStore
    {
        string Id { get; set; }

        long? StartTime { get; set; }

        long? EndTime { get; set; }

        string? Name { get; set; }

        string? Description { get; set; }

        public EventStore(string _Id, long? _StartTime, long? _EndTime, string? _Name, string? _Description)
        {
            Id = _Id;
            StartTime = _StartTime;
            EndTime = _EndTime;
            Name = _Name;
            Description = _Description;
        }
    }
}
