using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Uploader.Infrastructure.Data.Entities;

namespace Uploader.Infrastructure.Data.Contexts
{
    public class UploaderContext : DbContext
    {
        public UploaderContext(DbContextOptions<UploaderContext> options)
            : base(options)
        {
        }

        public DbSet<UploaderSettings> UploaderSettings { get; set; }

        public DbSet<UploaderFileExtension> UploaderFileExtensions { get; set; }

        public DbSet<EnabledUploaderSettings> EnabledUploaderSettings { get; set; }

        public DbSet<UploaderFile> UploaderFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UploaderSettings>()
                .Property(us => us.ConcurrencyStamp).IsConcurrencyToken();

            modelBuilder.Entity<UploaderFileExtension>()
                .Property(ufe => ufe.ConcurrencyStamp).IsConcurrencyToken();

            modelBuilder.Entity<EnabledUploaderSettings>()
                .HasKey(eus => eus.Id);

            modelBuilder.Entity<EnabledUploaderSettings>()
                .Property(eus => eus.ConcurrencyStamp).IsConcurrencyToken();
        }
    }
}
