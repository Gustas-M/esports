using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace esports.Data
{
    public class EsportsContext : DbContext
    {

        public EsportsContext(DbContextOptions<EsportsContext> options) : base(options)
        {

        }
        public DbSet<Models.Championship> Championships { get; set; }
        public DbSet<Models.Tournament> Tournaments { get; set; }
        public DbSet<Models.Match> Matches { get; set; }
    }
}