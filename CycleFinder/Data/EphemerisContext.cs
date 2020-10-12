using CycleFinder.Models;
using CycleFinder.Models.Planets;
using Microsoft.EntityFrameworkCore;

namespace CycleFinder.Data
{
    public class EphemerisContext : DbContext
    {
        public DbSet<Ephemeris> Ephemeris { get; set; }

        public EphemerisContext(DbContextOptions<EphemerisContext> options) : base(options) { }
    }
}
