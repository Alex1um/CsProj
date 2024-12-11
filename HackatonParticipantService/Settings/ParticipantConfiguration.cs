namespace HackatonParticipantService.Settings;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Hosting;
using HackatonBase.DataIO;
using HackatonBase.Participants;


public class ParticipantConfiguration : IHostedService
{
    // public int? Id { get; set; }
    // public string? Name { get; set; }
    public Participant Info;

    public Uri? HRManagerURL;

    private string? Hostname;
    public string? ParticipantType;
    private bool IsSetupNeeded = true;
    

    public ParticipantConfiguration(IConfiguration configuration)
    {
        Hostname = configuration.GetValue<string>("HOSTNAME");
        var name = configuration.GetValue<string>("Name");
        ParticipantType = configuration.GetValue<string>("PARTICIPANT") ?? "junior";
        if (name != null)
        {
            if (ParticipantType == "junior")
            {
                Info = new Junior() {
                    Name = name,
                    Id = configuration.GetValue<int>("Id")
                };
            } else {
                Info = new Teamlead() {
                    Name = name,
                    Id = configuration.GetValue<int>("Id")
                };
            }
            IsSetupNeeded = false;
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!IsSetupNeeded)
        {
            return;
        }
        DockerClient client = new DockerClientConfiguration(
            new Uri("unix://var/run/docker.sock"))
            .CreateClient();
        int containerIndex = int.Parse(
            (
                await client
                .Containers
                .ListContainersAsync(
                    new ContainersListParameters()
                    {
                        All = true
                    })
            )
            .First(c => c.ID[..12] == Hostname[..12])
            .Names
            .First()
            .Split("-")
            .Last()
        );
        if (ParticipantType == "junior")
        {
            var list = CSVReader.Read<Junior>("Juniors5.csv");
            var teamlead = list.First(t => t.Id == containerIndex);
            Info = new Junior {
                Name = teamlead.Name,
                Id = teamlead.Id
            };
        }
        else if (ParticipantType == "teamlead")
        {
            var list = CSVReader.Read<Teamlead>("Teamleads5.csv");
            var teamlead = list.First(t => t.Id == containerIndex);
            Info = new Teamlead {
                Name = teamlead.Name,
                Id = teamlead.Id
            };
        }
        Console.WriteLine($"{Info.Id} {Info.Name}");
        return;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        return;
    }

}