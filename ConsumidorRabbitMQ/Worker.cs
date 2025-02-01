using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ConsumidorRabbitMQ.Models;
using ConsumidorRabbitMQ.Services;

namespace ConsumidorRabbitMQ
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly EmailService _emailService;
        private IConnection _connection;
        private IModel _channel;
        private readonly string _queueName = "alertas_medicas";

        public Worker(ILogger<Worker> logger, EmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensaje = Encoding.UTF8.GetString(body);
                var resultado = JsonConvert.DeserializeObject<Resultado>(mensaje);

                _logger.LogInformation($"📩 Mensaje recibido: {mensaje}");

                // Verificar si los valores están fuera del rango normal
                if (resultado.Hemoglobina < 12 || resultado.Hemoglobina > 18 ||
                    resultado.Colesterol > 200 ||
                    resultado.GlobulosRojos < 4.2 || resultado.GlobulosRojos > 5.9)
                {
                    await _emailService.EnviarCorreo(
                        "paciente@dominio.com",
                        "⚠️ Alerta de Resultados Médicos",
                        $"📌 Los siguientes valores son anormales:\n" +
                        $"Hemoglobina: {resultado.Hemoglobina}\n" +
                        $"Colesterol: {resultado.Colesterol}\n" +
                        $"Glóbulos Rojos: {resultado.GlobulosRojos}\n" +
                        $"Consulte con su médico."
                    );
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
