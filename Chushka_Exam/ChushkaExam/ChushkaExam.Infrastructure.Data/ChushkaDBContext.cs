namespace ChushkaExam.Infrastructure.Data
{
    using ChushkaExam.Infrastructure.Models.ModelsAll;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class ChushkaDBContext : IdentityDbContext<ChushkaUser, IdentityRole<int>, int>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public ChushkaDBContext(DbContextOptions<ChushkaDBContext> options) : base(options)
        { }
    }
}