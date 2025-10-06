using Azure.Messaging.EventGrid;
using System.Text.Json.Serialization;

namespace ProductCentral.BackEnd.Models
{
    /// <summary>
    /// Abstract base class for all events in the ProductCentral system.
    /// Provides common event properties and functionality for EventGrid integration.
    /// </summary>
    public abstract class EventBase
    {
        /// <summary>
        /// Gets the type identifier for this event. Must be implemented by derived classes.
        /// </summary>
        [JsonIgnore()]
        public abstract string EventType { get; }
        
        /// <summary>
        /// Gets the data version for this event. Must be implemented by derived classes.
        /// </summary>
        [JsonIgnore()] 
        public abstract string DataVersion { get; }
        
        /// <summary>
        /// Gets the subject identifier for this event. Must be implemented by derived classes.
        /// </summary>
        [JsonIgnore()] 
        public abstract string Subject { get; }
        
        /// <summary>
        /// Gets or sets the UTC timestamp when this event was created.
        /// Defaults to the current UTC time when the event is instantiated.
        /// </summary>
        [JsonIgnore()] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Converts this event to an EventGrid event that can be published to Azure Event Grid.
        /// </summary>
        /// <returns>An <see cref="EventGridEvent"/> instance with the event data and metadata.</returns>
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