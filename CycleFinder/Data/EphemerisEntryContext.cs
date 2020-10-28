using CycleFinder.Extensions;
using CycleFinder.Models.Ephemeris;
using Microsoft.EntityFrameworkCore;

namespace CycleFinder.Data
{
    public class EphemerisEntryContext : DbContext
    {
        public DbSet<EphemerisEntry> DailyEphemeris { get; set; }

        public EphemerisEntryContext(DbContextOptions<EphemerisEntryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EphemerisEntry>().Property(x => x.Time).UsesUtc();
        }
    }
}
