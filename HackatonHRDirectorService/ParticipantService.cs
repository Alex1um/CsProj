using Docker.DotNet;
using Docker.DotNet.Models;
using HackatonBase.Participants;
using HackatonBase.DataIO;


public class ParticipantService : IHostedService
{
    public List<Uri> ParticipantsUri = [];

    public List<Junior> Juniors = CSVReader.Read<Junior>("Juniors5.csv");

    public List<Teamlead> Teamleads = CSVReader.Read<Teamlead>("Teamleads5.csv");

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        DockerClient client = new DockerClientConfiguration(
            new Uri("unix://var/run/docker.sock"))
            .CreateClient();
        ParticipantsUri = (
            (
                await client
                .Containers
                .ListContainersAsync(
                    new ContainersListParameters()
                    {
                        All = true
                    })
            )
            .Where(c => c.Names.Any(n => n.Contains("hackaton-participant")))
            .Select(c =>
            {
                Console.WriteLine(c.Names.First());
                return new Uri("http:/" + c.Names.First() + ":8080");
            })
            .ToList()
        );
        Console.WriteLine("Participants URI: " + string.Join(", ", ParticipantsUri));
        return;
    }


    public async Task StopAsync(CancellationToken cancellationToken)
    {
        return;
    }

}