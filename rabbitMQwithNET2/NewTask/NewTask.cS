using System;
using RabbitMQ.Client;
using System.Text;

class NewTask
{
    //string[] args
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "task_queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

           for(int i = 0; i < 25; i++) 
           {
                string message = $"Message {i} returning...";

                //var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                 routingKey: "task_queue",
                                 basicProperties: properties,
                                 body: body);

                Console.WriteLine(" [x] Sent to Consumer {0}", message);

                Thread.Sleep(1000);
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }

    private static string GetMessage(string[] args)
    {
        return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
    }
}