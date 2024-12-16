using HackatonBase.Models;
using HackatonBase.Participants;
using Microsoft.VisualBasic;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class HRDirectorRabbitMSendQService(IConfiguration configuration)
{
	private string _hostname = configuration.GetValue<string>("RABBITMQ_HOSTNAME") ?? "localhost";
	private string _username = configuration.GetValue<string>("RABBITMQ_USERNAME") ?? "user";
	private string _password = configuration.GetValue<string>("RABBITMQ_PASSWORD") ?? "password";
	private CreateChannelOptions channelOpts = new CreateChannelOptions(
	publisherConfirmationsEnabled: true,
	publisherConfirmationTrackingEnabled: true,
	outstandingPublisherConfirmationsRateLimiter: new ThrottlingRateLimiter(256));

	private BasicProperties props = new BasicProperties
	{
		Persistent = true,
	};

	public void SendJuniorStartMessage(HackatonAnnouncement<Junior> obj)
	{
		var message = JsonSerializer.Serialize(obj);
		SendMessage(message, "junior.start");
	}

	public void SendTeamleadStartMessage(HackatonAnnouncement<Teamlead> obj)
	{
		var message = JsonSerializer.Serialize(obj);
		SendMessage(message, "teamlead.start");
	}

	public async void SendMessage(string message, string routingKey)
	{
		var factory = new ConnectionFactory() { HostName = _hostname, UserName = _username, Password = _password };
		using (var connection = await factory.CreateConnectionAsync())
		using (var channel = await connection.CreateChannelAsync())
		{
			await channel.ExchangeDeclareAsync("StartExchange", ExchangeType.Topic);

			var body = Encoding.UTF8.GetBytes(message);

			await channel.BasicPublishAsync(exchange: "StartExchange",
						   routingKey: routingKey,
						   mandatory: true,
						   basicProperties: props,
						   body: body
				);
		}
	}
}