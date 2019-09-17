namespace EventuresApp.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order:BaseEntity<string>
    {
        public DateTime OrderedOn { get; set; }
        public string EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; }

        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public virtual AppUser Customer { get; set; }

        public int TicketCount { get; set; }
    }
}
