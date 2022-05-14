using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace DesafioBbb
{
    public static class RabbitMQObjects
    {
        public static void Declare(IConnection connection)
        {
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("votacao.bbb", RabbitMQ.Client.ExchangeType.Topic, true, false);
                channel.QueueDeclare("qq-coisa", true, false, false, null);
                channel.QueueBind("qq-coisa", "votacao.bbb", "#");
            }
        }
    }
}