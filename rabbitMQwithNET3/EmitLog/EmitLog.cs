using System;
using RabbitMQ.Client;
using System.Text;

class EmitLog
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            // Envia para todas filas 
            channel.ExchangeDeclare(exchange: "logs_exchange", type: ExchangeType.Fanout);

            for(int i = 0; i < 25; i++) {

                var message = GetMessage(args);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "logs_exchange",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);

                Thread.Sleep(2000);
            }
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }

    private static string GetMessage(string[] args)
    {
        return ((args.Length > 0)
               ? string.Join(" ", args)
               : "Hello World, Rafael!");
    }
}