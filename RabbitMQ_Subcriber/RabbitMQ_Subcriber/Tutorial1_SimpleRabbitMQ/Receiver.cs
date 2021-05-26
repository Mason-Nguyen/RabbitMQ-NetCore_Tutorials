namespace RabbitMQ_Subcriber.Tutorial1_SimpleRabbitMQ
{
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Text;

    public class Receiver
    {
        public static void ReceiveMessage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Make sure the queue exists before we try to consume messages from it.
            channel.QueueDeclare(queue: "hello",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            // provide a callback since message pushed up asynchronously
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received {0}", message);
            };

            channel.BasicConsume(queue: "hello",
                         autoAck: true,
                         consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
