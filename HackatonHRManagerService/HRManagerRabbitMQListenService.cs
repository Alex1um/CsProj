using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using HackatonBase.Models;
using HackatonBase.Participants;
using HackatonBase.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class HRManagerRabbitMQListenService(IConfiguration configuration, HRManager hrManager) : IHostedService
{
    private string _hostname = configuration.GetValue<string>("RABBITMQ_HOSTNAME") ?? "localhost";
    private string _username = configuration.GetValue<string>("RABBITMQ_USERNAME") ?? "user";
    private string _password = configuration.GetValue<string>("RABBITMQ_PASSWORD") ?? "password";


    private IConnection _connection;
    private IChannel _channel;

    private HRManager hrManager = hrManager;

    private BasicProperties props = new BasicProperties
    {
        Persistent = true,
    };

    private HackatonParticipantRegistration? DeserializeRegistration(BasicDeliverEventArgs ea)
    {
        var content = Encoding.UTF8.GetString(ea.Body.ToArray());
        var registration = JsonSerializer.Deserialize<HackatonParticipantRegistration>(content);
        return registration;
    }

    private async Task OnJuniorRegistrationMessage(object sender, BasicDeliverEventArgs ea)
    {
        var registration = DeserializeRegistration(ea);
        if (registration == null)
        {
            return;
        }
        hrManager.AddJuniorParticipant(registration.ParticipantInfo.Id, registration.Preferences.Select(p => p.Id).ToList(), registration.HackatonRunId);
    }

    private async Task OnTeamleadRegistrationMessage(object sender, BasicDeliverEventArgs ea)
    {
        var registration = DeserializeRegistration(ea);
        if (registration == null)
        {
            return;
        }
        hrManager.AddTeamleadParticipant(registration.ParticipantInfo.Id, registration.Preferences.Select(p => p.Id).ToList(), registration.HackatonRunId);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Thread.Sleep(5000);
        var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(exchange: "RegistrationExchange", type: ExchangeType.Topic, cancellationToken: cancellationToken);
        
        var junior_queue = await _channel.QueueDeclareAsync(queue: "JuniorRegistrationQueue", durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await _channel.QueueBindAsync(queue: junior_queue.QueueName, exchange: "RegistrationExchange", routingKey: "junior.registration", cancellationToken: cancellationToken);
        var juniorConsumer = new AsyncEventingBasicConsumer(_channel);
        juniorConsumer.ReceivedAsync += OnJuniorRegistrationMessage;
        await _channel.BasicConsumeAsync(queue: junior_queue.QueueName, autoAck: true, consumer: juniorConsumer, cancellationToken: cancellationToken);

        var teamlead_queue = await _channel.QueueDeclareAsync(queue: "teamleadRegistrationQueue", durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);
        await _channel.QueueBindAsync(queue: teamlead_queue.QueueName, exchange: "RegistrationExchange", routingKey: "teamlead.registration", cancellationToken: cancellationToken);
        var teamleadConsumer = new AsyncEventingBasicConsumer(_channel);
        teamleadConsumer.ReceivedAsync += OnTeamleadRegistrationMessage;
        await _channel.BasicConsumeAsync(queue: teamlead_queue.QueueName, autoAck: true, consumer: teamleadConsumer, cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

}