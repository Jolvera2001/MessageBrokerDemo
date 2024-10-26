using System.Text;
using RabbitMQ.Client;

namespace MessageBrokerDemo;

class Program
{
    public static void Main()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        
        // connection and channel
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        // declaring a queue
        string queueName = "test";
        channel.QueueDeclare(
            queue: queueName, 
            durable: false, 
            exclusive: false, 
            autoDelete: false, 
            arguments: null);

        while (true)
        {
            Console.WriteLine($"Enter a Message (or \"exit\" to quit) ");
            string message = Console.ReadLine();

            if (message.ToLower() == "exit")
            {
                break;
            }

            if (!string.IsNullOrEmpty(message))
            {
                var body = Encoding.UTF8.GetBytes(message);
                
                channel.BasicPublish(exchange: "",
                                routingKey: queueName,
                                basicProperties: null,
                                body: body);
                Console.WriteLine("Sent message: ", message);
            }
        }
    }
}