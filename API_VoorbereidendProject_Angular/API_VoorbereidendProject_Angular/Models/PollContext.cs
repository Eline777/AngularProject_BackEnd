using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_VoorbereidendProject_Angular.Models;

namespace API_VoorbereidendProject_Angular.Models
{
    public class PollContext : DbContext
    {
        public PollContext(DbContextOptions<PollContext> options) : base(options) { }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<Antwoord> Antwoorden { get; set; }
        public DbSet<Stem> Stemmen { get; set; }
        public DbSet<Vriendschap> Vriendschappen { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Poll>().ToTable("Poll");
            modelBuilder.Entity<Gebruiker>().ToTable("Gebruiker");
            modelBuilder.Entity<Antwoord>().ToTable("Antwoord");
            modelBuilder.Entity<Stem>().ToTable("Stem");
            modelBuilder.Entity<Vriendschap>().ToTable("Vriendschap");
        }
       // public DbSet<Vriendschap> Vriendschap { get; set; }
    }
}
