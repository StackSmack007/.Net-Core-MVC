using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Panda_Exam.Models;

namespace Panda_Exam.Data
{
    public abstract class UserContext : IdentityDbContext<User, IdentityRole, string>
    {
        public UserContext(DbContextOptions options) : base(options)
        { }

        protected UserContext()
        { }


    }
}