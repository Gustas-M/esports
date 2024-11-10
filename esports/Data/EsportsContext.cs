using esports.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace esports.Data
{
    public class EsportsContext : IdentityDbContext<User>
    {

        public EsportsContext(DbContextOptions<EsportsContext> options) : base(options)
        {

        }
        public DbSet<Models.Championship> Championships { get; set; }
        public DbSet<Models.Tournament> Tournaments { get; set; }
        public DbSet<Models.Match> Matches { get; set; }
        public DbSet<Models.Team> Teams { get; set; }
        public DbSet<Auth.Session> Sessions { get; set; }
    }
}