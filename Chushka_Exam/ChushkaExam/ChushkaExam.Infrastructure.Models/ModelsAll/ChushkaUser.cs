namespace ChushkaExam.Infrastructure.Models.ModelsAll
{
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    public class ChushkaUser:IdentityUser<int>
    {
        public ChushkaUser():base()
        {
            Orders = new HashSet<Order>();
        }

        [Required]
        [Column("Full Name")]
        public string FullName { get; set; }

        public virtual ICollection<Order> Orders { get; set; }


    }
}