namespace EventuresApp.Areas.Event.Models
{
using System;
using System.ComponentModel.DataAnnotations;
    public class eventInfoDtoOutput
    {
      //  [Required, MaxLength(64), MinLength(4)]
        public string Name { get; set; }

       //[Required(ErrorMessage = "Place must be provided!"), MaxLength(64, ErrorMessage = "Maximum Length of place Name is 64 symbols"), MinLength(4, ErrorMessage = "Minimum Length of place Name is 4 symbols")]
        public string Place { get; set; }

        public DateTime Start { get; set; }
    
        public DateTime End { get; set; }
    }
}