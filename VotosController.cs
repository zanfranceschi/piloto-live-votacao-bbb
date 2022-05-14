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
        private readonly ILogger<VotosController> _logger;

        public VotosController(IConnection connection, ILogger<VotosController> logger)
        {
            _logger = logger;
            _channel = connection.CreateModel();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Voto voto)
        {
            var payload = JsonSerializer.Serialize(voto);
            var messageBody = Encoding.UTF8.GetBytes(payload);

            _channel.BasicPublish("votacao.bbb", "voto", true, null, messageBody);
            
            _logger.LogDebug($"Mensagem publicada: {payload}");

            return Accepted(new { mensagem = $"Voto recebido pra {voto.Nome} 👍" });
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}