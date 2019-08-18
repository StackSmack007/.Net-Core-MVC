namespace Panda_Exam.Data
{
    using Microsoft.EntityFrameworkCore;
    using Panda_Exam.Models;

    public class PandaDbContext : UserContext
    {
        public PandaDbContext(DbContextOptions<PandaDbContext> options)
            : base(options)
        { }

        public DbSet<Package> Packages { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Receipt>()
                   .HasIndex(r => r.PackageId)
                   .IsUnique(true);

            base.OnModelCreating(builder);
        }
    }
}