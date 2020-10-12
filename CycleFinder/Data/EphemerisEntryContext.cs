using CycleFinder.Models;
using CycleFinder.Models.Ephemeris;
using Microsoft.EntityFrameworkCore;

namespace CycleFinder.Data
{
    public class EphemerisEntryContext : DbContext
    {
        public DbSet<EphemerisEntry> EphemerisEntries { get; set; }

        public EphemerisEntryContext(DbContextOptions<EphemerisEntryContext> options) : base(options) { }
    }
}
