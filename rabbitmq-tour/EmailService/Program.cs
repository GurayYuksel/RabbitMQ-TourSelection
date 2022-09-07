
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "bookings",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Trace.WriteLine(message);
    };

    channel.BasicConsume(queue: "bookings",
                         autoAck: true,
                         consumer: consumer);
}


