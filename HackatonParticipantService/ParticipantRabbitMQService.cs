using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using HackatonBase.Models;
using HackatonBase.Participants;
using HackatonBase.Extensions;
using HackatonParticipantService.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class ParticipantRabbitMQService(IConfiguration configuration, ParticipantConfiguration participantConfiguration) : IHostedService
{
    private string _hostname = configuration.GetValue<string>("RABBITMQ_HOSTNAME") ?? "localhost";
    private string _username = configuration.GetValue<string>("RABBITMQ_USERNAME") ?? "user";
    private string _password = configuration.GetValue<string>("RABBITMQ_PASSWORD") ?? "password";

    private ParticipantConfiguration _config = participantConfiguration;

    private IConnection _connection;
    private IChannel _channel;

    private BasicProperties props = new BasicProperties
    {
        Persistent = true,
    };

    private async Task SendRegistrationMessage(HackatonParticipantRegistration registration)
    {
        var jsoned = JsonSerializer.Serialize(registration);
        var content = Encoding.UTF8.GetBytes(jsoned);

        await _channel.BasicPublishAsync("RegistrationExchange", $"{registration.PatricipantType}.registration", true, props, content);
    }

    private async Task OnStartMessage(object sender, BasicDeliverEventArgs ea)
    {
        var content = Encoding.UTF8.GetString(ea.Body.ToArray());
        var participants = JsonSerializer.Deserialize<HackatonAnnouncement<Participant>>(content);
        if (participants == null)
        {
            return;
        }
        var shuffledParticipants = participants.Participants.GetShuffled();
        var hackatonParticipantRegistration = new HackatonParticipantRegistration
        {
            HackatonRunId = participants.HackatonRunId,
            Preferences = shuffledParticipants,
            PatricipantType = _config.ParticipantType ?? "junior",
            ParticipantInfo = _config.Info,
        };
        await SendRegistrationMessage(hackatonParticipantRegistration);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Thread.Sleep(5000);
        var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(exchange: "RegistrationExchange", type: ExchangeType.Topic, cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(exchange: "StartExchange", type: ExchangeType.Topic, cancellationToken: cancellationToken);

        var queue = await _channel.QueueDeclareAsync(durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

        await _channel.QueueBindAsync(queue: queue.QueueName, exchange: "StartExchange", routingKey: $"{_config.ParticipantType}.start", cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += OnStartMessage;
        // consumer.ReceivedAsync += async (model, ea) => {
        //     var content = Encoding.UTF8.GetString(ea.Body.ToArray());
        //     var participants = JsonSerializer.Deserialize<HackatonAnnouncement<Participant>>(content);
        //     if (participants == null) {
        //         return;
        //     }
        //     var shuffledParticipants = participants.Participants.GetShuffled();
        //     var hackatonParticipantRegistration = new HackatonParticipantRegistration {
        //         HackatonRunId = participants.HackatonRunId,
        //         Preferences = shuffledParticipants,
        //         PatricipantType = _config.ParticipantType ?? "junior",
        //         ParticipantInfo = _config.Info,
        //     };
        // };

        await _channel.BasicConsumeAsync(queue: queue.QueueName, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

}