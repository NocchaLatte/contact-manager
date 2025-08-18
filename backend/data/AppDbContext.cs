using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts => Set<Contact>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // using fluent API to configure the Contact entity(additional validation and ensure schema)
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Phone).HasMaxLength(15);
                entity.Property(c => c.Note).HasMaxLength(200);

                entity.HasIndex(c => c.Email).IsUnique(); // Ensure email uniqueness at the database level

            });
            



        }
    }
}
