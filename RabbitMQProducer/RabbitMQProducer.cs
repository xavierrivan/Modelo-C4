using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;

public class RabbitMQProducer
{
    private const string ExchangeName = "ResultadosExchange";

    public void Publish(Resultado resultado)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" }; // Cambiar si es un contenedor Docker
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Fanout);

            var message = JsonConvert.SerializeObject(resultado);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: ExchangeName, routingKey: "", basicProperties: null, body: body);

            Console.WriteLine($"[x] Enviado: {message}");
        }
    }
}

