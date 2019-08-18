namespace Panda_Exam.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    public class User : IdentityUser<string>
    {
        public User():base()
        {
            Receipts = new HashSet<Receipt>();
            Packages = new HashSet<Package>();
        }
        public string nickNameTag { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<Package> Packages { get; set; }
    }
}