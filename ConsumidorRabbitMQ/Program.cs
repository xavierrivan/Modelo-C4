using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConsumidorRabbitMQ.Services;
using ConsumidorRabbitMQ;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<EmailService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
