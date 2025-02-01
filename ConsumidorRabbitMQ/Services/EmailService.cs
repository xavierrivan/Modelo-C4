using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ConsumidorRabbitMQ.Services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task EnviarCorreo(string destinatario, string asunto, string cuerpo)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.tudominio.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("tucorreo@tudominio.com", "tucontraseña"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("tucorreo@tudominio.com"),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(destinatario);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($" Correo enviado a {destinatario}");
            }
            catch (Exception ex)
            {
                _logger.LogError($" Error al enviar correo: {ex.Message}");
            }
        }
    }
}
