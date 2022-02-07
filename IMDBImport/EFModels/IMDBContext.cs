using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IMDBImport.EFModels
{
    //To generate the Context and EFModel class in NuGet Console:
    //Scaffold-DbContext "Server=localhost;Database=IMDB;User Id=imdb_user;Password=ImDbWuUu;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

    public partial class IMDBContext : DbContext
    {
        public IMDBContext()
        {
        }

        public IMDBContext(DbContextOptions<IMDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TitlesBasic> TitlesBasics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=IMDB;User Id=imdb_user;Password=ImDbWuUu;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Danish_Norwegian_CI_AS");

            modelBuilder.Entity<TitlesBasic>(entity =>
            {
                entity.HasKey(e => e.Tconst);

                entity.ToTable("TitlesBasic");

                entity.Property(e => e.Tconst)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OriginalTitle)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryTitle)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.TitleType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
