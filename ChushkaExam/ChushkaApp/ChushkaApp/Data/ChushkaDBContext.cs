namespace ChushkaApp.Data
{
    using ChushkaApp.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class ChushkaDBContext : IdentityDbContext<ChushkaUser>
    {
        public ChushkaDBContext(DbContextOptions<ChushkaDBContext> options)
            : base(options)
        { }
        public ChushkaDBContext() : base()
        { }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}