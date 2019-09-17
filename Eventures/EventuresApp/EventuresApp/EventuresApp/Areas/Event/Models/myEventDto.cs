namespace EventuresApp.Areas.Event.Models
{
    using System;
    public class myEventDto
    {
        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TotalTicketCount { get; set; }
    }
}
