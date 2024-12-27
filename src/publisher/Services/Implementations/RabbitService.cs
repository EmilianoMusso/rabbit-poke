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

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);

                await Console.Out.WriteLineAsync(response);
            };

            await channel.BasicConsumeAsync(queue: message.ReplyTo,
                                 autoAck: true,
                                 consumer: consumer);
        }

        var messageBytes = Encoding.UTF8.GetBytes(message.Body);

        await channel.BasicPublishAsync(exchange: message.Exchange ?? message.RoutingKey,
                             message.RoutingKey,
                             mandatory: true,
                             basicProperties: props,
                             body: messageBytes);

        await Task.CompletedTask;
    }
}
