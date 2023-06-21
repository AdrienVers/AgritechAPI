using agricultureAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace agricultureAPI.Data
{
    public class CropDbContext : DbContext
    {
        public DbSet<Crop> Crops { get; set; }

        public CropDbContext(
            DbContextOptions<CropDbContext> options)
            : base(options) 
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Crop>(
                crop =>
                {
                    crop.ToTable("Crops");
                    crop.HasKey(crop => crop.Id);
                    crop.Property(crop => crop.Title).HasMaxLength(1024);
                }
            );
        }
    }
}
