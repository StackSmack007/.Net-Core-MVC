namespace EventuresApp.Areas.Event.Models
{
    using EventuresApp.Services.MapperAutoConfiguration;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class eventCreateDto : eventInfoDtoOutput, IValidatableObject, IMapTo<EventuresApp.Models.Event>
    {
        public int TotalTickets { get; set; }

        public decimal PricePerTicket { get; set; }

        private const string defaultDate = "Fri, 19 Aug 2011 13:45:00 GMT";


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.Name)) yield return new ValidationResult("Event Name must not be null or empty!");
            if (this.Name.Length < 10) yield return new ValidationResult("Event Name must be longer or equal to 10 symbols");
            if (string.IsNullOrEmpty(this.Place)) yield return new ValidationResult("Event Place must not be null or empty!");
            if (this.Start.ToString("R") == defaultDate) yield return new ValidationResult("StartDate must be a valid date, provide another!");
            if (this.End.ToString("R") == defaultDate) yield return new ValidationResult("EndDate must be a valid date, provide another!");
            if (Start >= End) yield return new ValidationResult("EndDate must be after the start date");
            if (this.TotalTickets <= 0) yield return new ValidationResult("TotalTickets must be positive");
            if (this.TotalTickets > int.MaxValue) yield return new ValidationResult("TotalTickets must be no more than 2147483647");
            if (this.PricePerTicket <= 0) yield return new ValidationResult("Price for a ticket must be positive");
            if (this.PricePerTicket > decimal.MaxValue) yield return new ValidationResult("Price for a ticket must be below 79228162514264337593543950335");

        }
    }
}