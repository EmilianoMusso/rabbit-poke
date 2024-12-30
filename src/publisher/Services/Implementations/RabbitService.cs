using poke.Models;
using poke.Services.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace poke.Services.Implementations;
public class RabbitService : IRabbitService
{
    public async Task PublishAsync(Message message)
    {
        var factory = new ConnectionFactory
        {
            HostName = message.HostName
        };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var props = new BasicProperties()
        {
            CorrelationId = Guid.NewGuid().ToString(),
            Type = message.RoutingKey
        };

        if (!string.IsNullOrEmpty(message.ReplyTo))
        {
            props.ReplyTo = message.ReplyTo;
            await DeclareConsumer(channel, message.ReplyTo);
        }

        var messageBytes = Encoding.UTF8.GetBytes(message.Body);

        await channel.BasicPublishAsync(
            exchange: message.Exchange,
            message.RoutingKey,
            mandatory: false,
            basicProperties: props,
            body: messageBytes);
    }

    private static async Task DeclareConsumer(IChannel channel, string replyTo)
    {
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var responseBody = ea.Body.ToArray();
            var responseString = Encoding.UTF8.GetString(responseBody);

            await Console.Out.WriteLineAsync(responseString);

            await channel.BasicConsumeAsync(queue: replyTo,
                                 autoAck: true,
                                 consumer: consumer);
        };

        await channel.BasicConsumeAsync(queue: replyTo, autoAck: true, consumer: consumer);
    }
}