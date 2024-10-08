﻿namespace HackatonService;
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Hosting; 
using HackatonService.Settings;

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

                services.AddOptions<HackatonSettings>().Bind(hostContext.Configuration.GetSection("Hackaton"));
                services.AddOptions<DataSourceSettings>().Bind(hostContext.Configuration.GetSection("Sources"));
            })
            .Build();
        host.Run();
    }
}