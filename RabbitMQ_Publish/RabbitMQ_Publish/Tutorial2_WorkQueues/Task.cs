namespace RabbitMQ_Publish.Tutorial2_WorkQueues
{
    using RabbitMQ.Client;
    using System;
    using System.Text;

    public class Task
    {
        public static void SendMessage(string[] value)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                // durable: true - queue won't be lost even if RabbitMQ restarts
                // avoid lost messages when RMQ crash or Restart since in that cases - a queue do not exists
                channel.QueueDeclare(queue: "task_queue",
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

                var message = GetMessage(value);
                var body = Encoding.UTF8.GetBytes(message);

                // This tells RabbitMQ not to give more than one message to a worker at a time.
                // Or, in other words, don't dispatch a new message to a worker until it has processed and acknowledged the previous one.
                // Instead, it will dispatch it to the next worker that is not still busy.
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                // Messages won't be lost - it is persistent - message written to disk
                // Caution: If you need a stronger guarantee then you can use -publisher confirms-.
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                             routingKey: "task_queue",
                             basicProperties: null,
                             body: body);

                Console.WriteLine("Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private static string GetMessage(string[] value)
            => ((value.Length > 0) ? string.Join(" ", value) : "Hello World!");
    }
}
