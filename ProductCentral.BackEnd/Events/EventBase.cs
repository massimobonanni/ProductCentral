using Azure.Messaging.EventGrid;
using System.Text.Json.Serialization;

namespace ProductCentral.BackEnd.Models
{
    public abstract class EventBase
    {
        [JsonIgnore()]
        public abstract string EventType { get; }
        [JsonIgnore()] 
        public abstract string DataVersion { get; }
        [JsonIgnore()] 
        public abstract string Subject { get; }
        [JsonIgnore()] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual EventGridEvent ToEventGridEvent()
        {
            return new EventGridEvent(
                subject: this.Subject,
                eventType: this.EventType,
                dataVersion: this.DataVersion,
                data: this)
            {
                Id = Guid.NewGuid().ToString(),
                EventTime = this.CreatedAt
            };
        }
    }
}