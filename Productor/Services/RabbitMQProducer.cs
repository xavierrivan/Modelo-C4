using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using Productor.Models;
using Microsoft.AspNetCore.Connections;

namespace Productor.Services
{
    public class RabbitMQProducer
    {
        private readonly string _hostName = "localhost";
        private readonly string _queueName = "alertas_medicas";

        public void PublicarMensaje(Resultado resultado)
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: _queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var mensaje = JsonConvert.SerializeObject(resultado);
            var body = Encoding.UTF8.GetBytes(mensaje);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
        }
    }
}
