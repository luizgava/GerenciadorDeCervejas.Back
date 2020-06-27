using System.Text;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Mensageria.Infra;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace GerenciadorDeCervejas.Mensageria.Eventos
{
    public class EventoNotificarAlteracaoCerveja : IEventoNotificarAlteracaoCerveja
    {
        #region ctor
        private readonly ConfiguracoesRabbitMq _configuracoesRabbitMq;

        public EventoNotificarAlteracaoCerveja(IOptions<ConfiguracoesRabbitMq> configuracoesRabbitMqOptions)
        {
            _configuracoesRabbitMq = configuracoesRabbitMqOptions.Value;
        }
        #endregion

        #region Publicar
        public void Publicar(Cerveja cerveja)
        {
            var fabricaConexao = new ConnectionFactory { HostName = _configuracoesRabbitMq.Servidor, UserName = _configuracoesRabbitMq.Usuario, Password = _configuracoesRabbitMq.Senha };

            using var conexao = fabricaConexao.CreateConnection();
            using var canal = conexao.CreateModel();

            canal.QueueDeclare(_configuracoesRabbitMq.NomeFila, false, false, false);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cerveja));
            canal.BasicPublish("", _configuracoesRabbitMq.NomeFila, null, body);
        }
        #endregion
    }
}
