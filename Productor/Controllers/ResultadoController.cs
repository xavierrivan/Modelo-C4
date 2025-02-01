using Microsoft.AspNetCore.Mvc;
using Productor.Models;
using Productor.Services;
using Productor.Utils;

namespace Productor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultadoController : ControllerBase
    {
        private readonly RabbitMQProducer _rabbitMQProducer;

        public ResultadoController()
        {
            _rabbitMQProducer = new RabbitMQProducer();
        }

        [HttpPost]
        public IActionResult CrearResultado([FromBody] Resultado resultado)
        {
            if (resultado == null)
                return BadRequest("Datos inválidos");

            if (ValidationHelper.EsAnormal(resultado))
            {
                _rabbitMQProducer.PublicarMensaje(resultado);
                return Ok("Resultado anormal detectado. Notificación enviada.");
            }

            return Ok("Resultado normal.");
        }
    }
}
