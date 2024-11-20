namespace HackatonParticipantService.Settings;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Hosting;

public class ParticipantConfiguration : IHostedService
{
    public int? Id { get; set; }
    public string? Name { get; set; }

    private string? Hostname;

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

    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!IsSetupNeeded) 
        {
            return;
        }
        Console.WriteLine("Setup begin");
        // DockerClient client = new DockerClientConfiguration(
        //     new Uri("unix:///var/run/docker.sock"))
        //     .CreateClient();
        // IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(
        //     new ContainersListParameters(){
        //         Limit = 10,
        //     });
        return;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        return;
    }

}