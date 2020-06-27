using GerenciadorDeCervejas.Infra.Entidades;

namespace GerenciadorDeCervejas.Dominio.Cervejas
{
    public class Cerveja : EntidadeBase
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Harmonizacao { get; set; }
        public string Cor { get; set; }
        public string TeorAlcoolico { get; set; }
        public string Temperatura { get; set; }
        public string Ingredientes { get; set; }
    }
}