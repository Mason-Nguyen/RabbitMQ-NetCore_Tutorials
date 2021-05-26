using RabbitMQ_Subcriber.Tutorial1_SimpleRabbitMQ;
using RabbitMQ_Subcriber.Tutorial2_WorkQueues;

namespace RabbitMQ_Subcriber
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tutorial 1 - Simple RabbitMQ
            // Receiver.ReceiveMessage();

            // Tutorial 2 - Work Queues
            Worker.ReceiveMessage();
        }
    }
}
