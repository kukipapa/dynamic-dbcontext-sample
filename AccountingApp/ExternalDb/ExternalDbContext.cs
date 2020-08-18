using Microsoft.EntityFrameworkCore;

namespace AccountingApp.ExternalDb
{
    public class ExternalDbContext : DbContext
    {
        public virtual DbSet<MyTable> MyTable { get; set; }

        public ExternalDbContext(string connectionString)
            : base(new DbContextOptionsBuilder<ExternalDbContext>().UseSqlServer(connectionString).Options)
        {
        }

        public ExternalDbContext(DbContextOptions<ExternalDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MyTable>(entity =>
            {
                entity.Property(e => e.Data).HasColumnType("text");
            });
        }
    }
}
