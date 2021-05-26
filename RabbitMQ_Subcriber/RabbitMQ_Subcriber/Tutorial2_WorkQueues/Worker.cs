using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ_Subcriber.Tutorial2_WorkQueues
{
    public class Worker
    {
        public static void ReceiveMessage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // durable: true - queue won't be lost even if RabbitMQ restarts
            // avoid lost messages when RMQ crash or Restart since in that cases - a queue do not exists
            channel.QueueDeclare(queue: "task_queue",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null); ;

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                //int dots = message.Split('.').Length - 1;
                //Thread.Sleep(dots * 1000);

                Console.WriteLine(" [x] Done");

                // Manual ack message - when autoAck = false
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            // AutoAck: true - Auto ack messgae auto send to RMQ to inform message is received and RMQ can delete it.
            /* AutoAck: false - manually ack - only send ack message to RMQ when we done task which hanlde message.
               - Remember call channel.BasicAck to manual send ack.
                If a consumer dies (its channel is closed, connection is closed, or TCP connection is lost) without sending an ack, 
                RabbitMQ will understand that a message wasn't processed fully and will re-queue it. 
                If there are other consumers online at the same time, it will then quickly redeliver it to another consumer. 
                That way you can be sure that no message is lost, even if the workers occasionally die.
            */
            channel.BasicConsume(queue: "task_queue", autoAck: true, consumer: consumer);

            Console.WriteLine(" Wait for receive message...");
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
