using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RabbitMQ.Client;
using System.Text;

public interface IMessageProducer
{
    void SendMessage(string message);
}

public class RabbitMQProducer : IMessageProducer
{
    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "bookings",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", routingKey: "bookings", body: body);
    }
}

namespace RabbitMQ_TourSelection.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMessageProducer _messageProducer;

        public IndexModel(ILogger<IndexModel> logger, IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        public void OnGet()
        {

        }

        public void SendMessage()
        {
            _messageProducer.SendMessage("Hej");
        }
    }
}