namespace EventuresApp.Areas.Event.Models
{
using System;
using System.ComponentModel.DataAnnotations;
    public class eventCreateDto:eventInfoDtoOutput
    {
        public int TotalTickets { get; set; }
        [Range(typeof(decimal), "0", "79228162514264337593543950335", ErrorMessage = "Ticket price must be positive!")]
        public decimal PricePerTicket { get; set; }
    }
}