using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace esports.Data
{
    public static class Migrations
    {

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();

            using EsportsContext dbContext = serviceScope.ServiceProvider.GetRequiredService<EsportsContext>();

            dbContext.Database.Migrate();
        }
    }
}