using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Mensageria.Infra;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GerenciadorDeCervejas.Mensageria.Escutas
{
    public class EscutaEventoNotificarAlteracaoCerveja : BackgroundService
    {
        #region ctor
        private readonly ConfiguracoesRabbitMq _configuracoesRabbitMq;
        private readonly IConnection _conexao;
        private readonly IModel _canal;
        private readonly ILogger _logger;

        public EscutaEventoNotificarAlteracaoCerveja(IOptions<ConfiguracoesRabbitMq> configuracoesRabbitMqOptions,
                                                     ILogger<Cerveja> logger)
        {
            _configuracoesRabbitMq = configuracoesRabbitMqOptions.Value;
            var fabricaConexao = new ConnectionFactory { HostName = _configuracoesRabbitMq.Servidor, UserName = _configuracoesRabbitMq.Usuario, Password = _configuracoesRabbitMq.Senha };

            _conexao = fabricaConexao.CreateConnection();
            _canal = _conexao.CreateModel();
            _canal.QueueDeclare(_configuracoesRabbitMq.NomeFila, false, false, false);
            _logger = logger;
        }
        #endregion

        #region Iniciar
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumidor = new EventingBasicConsumer(_canal);
            consumidor.Received += (sender, args) =>
            {
                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var cerveja = JsonConvert.DeserializeObject<Cerveja>(json);

                _logger.Log(LogLevel.Information, $"Alterou a cerveja {cerveja.Nome} na data {cerveja.DataAlteracao:dd/MM/yyyy HH:mm:ss}.");

                _canal.BasicAck(args.DeliveryTag, false);
            };
            _canal.BasicConsume(_configuracoesRabbitMq.NomeFila, false, consumidor);

            return Task.CompletedTask;
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            _canal.Close();
            _conexao.Close();
            base.Dispose();
        }
        #endregion
    }
}
