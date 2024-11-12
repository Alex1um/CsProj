namespace HackatonService;
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Configuration; 
using HackatonService.Settings;
using HackatonService.DB;
using Npgsql;
using Microsoft.EntityFrameworkCore;

static class Program
{
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) => {
                services.AddHostedService<HackatonWorker>();
                services.AddTransient<Hackaton>();
                services.AddTransient<ITeamBuildingStrategy, RandomTeamBuildingStrategy>();
                services.AddTransient<ITeamBuildingStrategy, StableMarriageTeamBuildingStrategy>();
                services.AddTransient<HRManager>();
                services.AddTransient<HRDirector>();
                services.AddDbContextPool<HackatonDbContext>(options =>
                {
                    options.UseNpgsql(hostContext.Configuration.GetSection("Database").GetSection("PostgresConnection").Value);
                });

                services.AddOptions<HackatonSettings>().Bind(hostContext.Configuration.GetSection("Hackaton"));
                services.AddOptions<DataSourceSettings>().Bind(hostContext.Configuration.GetSection("Sources"));
                services.AddOptions<DataSourceSettings>().Bind(hostContext.Configuration.GetSection("Database"));
            })
            .Build();
        host.Run();
    }
}