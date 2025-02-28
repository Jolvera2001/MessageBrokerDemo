﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageReceiver;

public class Program
{
    public static void Main()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        string queueName = "test";
        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        Console.WriteLine($"Waiting for messages...");

        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Received message: {message}");
        };
        
        channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        Console.WriteLine("Press [enter] to exit...");
        Console.ReadLine();
    }
}