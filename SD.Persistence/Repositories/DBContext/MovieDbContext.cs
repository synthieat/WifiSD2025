using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SD.Core.Entities.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SD.Persistence.Repositories.DBContext
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext() { }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) {

            Database.SetCommandTimeout(90);
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<MediumType> MediumTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable(nameof(Movie) + "s");
                /* Nicht notwendig, weil Id implizit als Schlüssel erkannt wird
                entity.HasKey(e => e.Id);
                */
                entity.Property(p => p.Title).HasMaxLength(128).IsRequired();
                entity.HasIndex(p => p.Title).HasDatabaseName("IX_" + nameof(Movie) + "s_" + nameof(Movie.Title));
                entity.HasIndex(p => p.GenreId).HasDatabaseName("IX_" + nameof(Movie) + "s_" + nameof(Movie.GenreId));
                entity.HasIndex(p => p.MediumTypeCode).HasDatabaseName("IX_" + nameof(Movie) + "s_" + nameof(Movie.MediumTypeCode));
                entity.Property(p => p.ReleaseDate).HasColumnType("date");
                entity.Property(p => p.Price).HasPrecision(18, 2).HasDefaultValue(0M);

            });

            modelBuilder.Entity<MediumType>(entity =>
            {
                entity.ToTable(nameof(MediumType) + "s")
                      .HasKey(nameof(MediumType.Code));

            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasOne(m => m.MediumType)  
                      .WithMany(mt => mt.Movies)
                      .HasForeignKey(m => m.MediumTypeCode)   
                      .OnDelete(DeleteBehavior.SetNull); /* Beim Löschen einers Mediumtypes, wird der
                                                          * Mediumcode in der Movie Tabelle auf NULL gesetzt.*/
            });

            /* Alternativ: Direkt über Entity, ohne Lambda */
            //modelBuilder.Entity<Movie>()
            //            .HasOne(m => m.MediumType)
            //            .WithMany(mt => mt.Movies)
            //            .HasForeignKey(m => m.MediumTypeCode)
            //            .OnDelete(DeleteBehavior.SetNull); /* Beim Löschen einers Mediumtypes, wird der
            //                                                * Mediumcode in der Movie Tabelle auf NULL gesetzt.*/
                      
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable(nameof(Genre) + "s");
                entity.HasMany(g => g.Movies)
                      .WithOne(m => m.Genre)
                      .HasForeignKey(m => m.GenreId)
                      .OnDelete(DeleteBehavior.Restrict); /* Keine Löschweitergabe */                    
            });

            /* Standarddaten in die Datenbank schreiben */

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action" },
                new Genre { Id = 2, Name = "Horror"},
                new Genre { Id = 3, Name = "Science Fiction" },
                new Genre { Id = 4, Name = "Comedy" }
            );

            modelBuilder.Entity<MediumType>().HasData(
                new MediumType { Code = "VHS", Name = "Video Home System" },
                new MediumType { Code = "DVD", Name = "Digital Versatile Disc" },
                new MediumType { Code = "BR", Name = "Blu-ray Disc" },
                new MediumType { Code = "BR3D", Name = "3D Blu-ray Disc" },
                new MediumType { Code = "BR4K", Name = "4K Blu-ray Disc" }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie
                {
                    Id =  new Guid("eeb15090-d5a6-46b9-a1a2-59eafb99ad24"),
                    Title = "Rambo",
                    Price = 4.9M,
                    MediumTypeCode = "VHS",
                    ReleaseDate = new DateTime(1985, 4, 13),
                    GenreId = 1,
                    Rating = Ratings.Medium
                },
                new Movie
                {
                    Id = new Guid("92436ae8-7c68-482a-9230-f289e53e1156"),
                    Title = "Star Trek - Beyond",
                    Price = 14.9M,
                    MediumTypeCode = "BR3D",
                    ReleaseDate = new DateTime(2016, 5, 30),
                    GenreId = 3,
                    Rating = Ratings.Great
                },
                 new Movie
                 {
                     Id = new Guid("15614c54-4523-4ad0-87bf-8579f38f793b"),
                     Title = "Star Wars - Episode IV",
                     Price = 9.9M,
                     MediumTypeCode = "DVD",
                     ReleaseDate = new DateTime(1987, 4, 13),
                     GenreId = 3,
                     Rating = Ratings.Medium
                 },
                  new Movie
                  {
                      Id = new Guid("b4aa56ae-dc15-41dd-988f-dbaa5d0f510e"),
                      Title = "The Ring",
                      Price = 12.9M,
                      MediumTypeCode = "BR",
                      ReleaseDate = new DateTime(2005, 11, 15),
                      GenreId = 2,
                      Rating = Ratings.Bad
                  }
            );            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

#if DEBUG
            /* Verzeichnis auf Root des Projekts kürzen */
            if(currentDirectory.IndexOf("bin") > -1)
            {
                currentDirectory = currentDirectory.Substring(0, currentDirectory.IndexOf("bin"));
            }
#endif

            var configurationBuilder = new ConfigurationBuilder().SetBasePath(currentDirectory)
                                                                 .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                                                                 .AddUserSecrets(Assembly.GetExecutingAssembly());

            var configuration = configurationBuilder.Build();
            var connectionString = configuration.GetConnectionString("MovieDbContext");
            optionsBuilder.UseSqlServer(connectionString, opts => opts.CommandTimeout(60));


        }
    }
}
