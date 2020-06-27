namespace GerenciadorDeCervejas.Mensageria.Infra
{
    public class ConfiguracoesRabbitMq
    {
        public string Servidor { get; set; }
        public string NomeFila { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }
}
