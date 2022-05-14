using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace DesafioBbb
{
    [ApiController]
    [Route("[controller]")]
    public class VotosController
        : ControllerBase, IDisposable
    {
        private IModel _channel;

        public VotosController(IConnection connection)
        {
            _channel = connection.CreateModel();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Votos voto)
        {
            var payload = JsonSerializer.Serialize(voto); // string
            var messageBody = Encoding.UTF8.GetBytes(payload); // array de bytes

            _channel.BasicPublish("votacao.bbb", "#", true, null, messageBody);

            return Ok(new { mensagem = $"Voto recebido pra {voto.Nome} 👍" });
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }

    public class Votos
    {
        public string Nome { get; set; }


    }
}