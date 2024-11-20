namespace HackatonParticipantService.Settings;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Hosting;
using HackatonBase.DataIO;
using HackatonBase.Participants;


public class ParticipantConfiguration : IHostedService
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    private string? Hostname;
    private string? ParticipantType;
    private bool IsSetupNeeded = true;

    public ParticipantConfiguration(IConfiguration configuration)
    {
        Hostname = configuration.GetValue<string>("HOSTNAME");
        var name = configuration.GetValue<string>("Name");
        if (name != null)
        {
            Name = name;
            Id = configuration.GetValue<int>("Id");
            IsSetupNeeded = false;
        }
        ParticipantType = configuration.GetValue<string>("PARTICIPANT") ?? "junior";
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
            Name = teamlead.Name;
            Id = teamlead.Id;
        }
        else if (ParticipantType == "teamlead")
        {
            var list = CSVReader.Read<Teamlead>("Teamleads5.csv");
            var teamlead = list.First(t => t.Id == containerIndex);
            Name = teamlead.Name;
            Id = teamlead.Id;
        }
        Console.WriteLine($"{Id} {Name}");
        return;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        return;
    }

}