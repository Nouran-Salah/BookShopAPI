using Microsoft.EntityFrameworkCore;
using DAl.models;

namespace DAL.models
{
    public class ProgramContext : DbContext
    {

        public ProgramContext(DbContextOptions<ProgramContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
              .ToTable("Categories", "MasterSchema");
            modelBuilder.Entity<Category>()
                .HasKey(c => c.Id);
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.catName)
                .IsUnique();
            modelBuilder.Entity<Category>()
                .Property(c => c.catName)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Category>()
                .Property(c => c.catOrder)
                .IsRequired();
            modelBuilder.Entity<Category>()
                .Property(c => c.markedAsDeleted)
                .HasColumnName("is deleted")
                .HasDefaultValue(false);
            modelBuilder.Entity<Category>()
                .Ignore(c => c.createdDate);


            modelBuilder.Entity<Product>()
               .ToTable("Product", "MasterSchema");

            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Product>()
                .Property(p => p.Title).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.Description).HasMaxLength(250);
            modelBuilder.Entity<Product>()
                .Property(p => p.Author).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Product>()
                .Property(p => p.price).IsRequired().HasColumnName("ProductPrice");


        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
