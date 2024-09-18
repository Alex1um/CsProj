
namespace CsProj.src.ObjectOriented;
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.Extensions.Hosting; 

static class Program
{
    const int TRIES = 1;
    public static void Main(string[] args)
    {

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) => {
                services.AddHostedService<HackatonWorker>();
                services.AddTransient<Hackaton>(_ => new Hackaton("./CSHARP_2024_NSU/Teamleads20.csv", "./CSHARP_2024_NSU/Juniors20.csv"));
                services.AddTransient<ITeamBuildingStrategy, RandomTeamBuildingStrategy>();
                services.AddTransient<HRManager>();
                services.AddTransient<HRDirector>();
            })
            .Build();
        host.Run();

        // var hackaton = new Hackaton("./CSHARP_2024_NSU/Teamleads20.csv", "./CSHARP_2024_NSU/Juniors20.csv");
        // var manager = new HRManager(new StableMarriageTeamBuildingStrategy());
        // var director = new HRDirector();

        // var meanHarmonicsSum = 0.0;
        // for (int i = 0; i < TRIES; i++)
        // {
        //     meanHarmonicsSum = hackaton.Run(manager, director);
        // }

        // Console.WriteLine(meanHarmonicsSum / TRIES);
    }
}