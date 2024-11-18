using Microsoft.EntityFrameworkCore;
using PTV.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PTV.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Street> Streets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Street>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Geometry)
                      .HasColumnType("geometry(LineString, 4326)"); // Geometry JSON olarak saklanır
            });
        }
    }
}
