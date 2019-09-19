namespace EventuresApp.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class AppUser:IdentityUser
    {
        public AppUser()
        {
            Orders = new HashSet<Order>();
        }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        [Required]
        public string UCN { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}