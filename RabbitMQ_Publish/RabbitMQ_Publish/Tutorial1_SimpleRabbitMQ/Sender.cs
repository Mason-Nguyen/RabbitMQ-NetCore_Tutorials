using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ_Publish.Tutorial1
{
    public class Sender
    {
        public void SendMessage()
        {
            // Connect to a RabbitMQ node on the local machine - hence the localhost
            // Specify its hostname or IP address here if wanted to connect to a node on a different machine 
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            // A channel, which is where most of the API for getting things done resides.
            using (var channel = connection.CreateModel())
            {
                //  a queue is idempotent - it will only be created if it doesn't exist already
                channel.QueueDeclare(queue: "hello",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

                string message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);

                Console.WriteLine("Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
