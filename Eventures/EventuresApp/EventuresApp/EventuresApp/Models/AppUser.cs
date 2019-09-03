﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EventuresApp.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        [Required]
        public string UCN { get; set; }
    }
}