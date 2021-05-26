using RabbitMQ_Publish.Tutorial2_WorkQueues;

namespace RabbitMQ_Publish
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tutorial 1 - Simple RabbitMQ
            //var sender = new Sender();
            //sender.SendMessage();

            // Tutorial 2 _ Work Queues
            Task.SendMessage(args);
        }
    }
}
