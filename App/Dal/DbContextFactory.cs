using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using EventApp.App.Configuration;

namespace EventApp.App.Dal;

public class DbContextFactory : IDesignTimeDbContextFactory<EventAppContext>
{
    public EventAppContext CreateDbContext(string[] args)
    {
        //Fix datetime conversion refs: https://stackoverflow.com/questions/69961449/net6-and-datetime-problem-cannot-write-datetime-with-kind-utc-to-postgresql-ty
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        var configuration = ConfigurationLoader.Configure();
        var optionsBuilder = new DbContextOptionsBuilder<EventAppContext>();
        
        optionsBuilder.UseNpgsql(configuration.GetValue<string>("Pgsql"));

        return new EventAppContext(optionsBuilder.Options);
    }
}
