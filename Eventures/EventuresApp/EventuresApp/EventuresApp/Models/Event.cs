namespace EventuresApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Event:BaseEntity<string>
    {
        public Event()
        {
            Orders = new HashSet<Order>();
        }

        [Required, MaxLength(64),MinLength(4)]
        public string Name { get; set; }

        [Required, MaxLength(64), MinLength(4)]
        public string Place { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TotalTickets { get; set; }

        [Range(typeof(decimal),"0", "79228162514264337593543950335")]
        public decimal PricePerTicket { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}