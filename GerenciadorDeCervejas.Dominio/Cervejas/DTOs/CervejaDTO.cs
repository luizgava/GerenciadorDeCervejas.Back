using Microsoft.Extensions.FileProviders;

namespace GerenciadorDeCervejas.Dominio.Cervejas.DTOs
{
    public class CervejaDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Harmonizacao { get; set; }
        public string Cor { get; set; }
        public string TeorAlcoolico { get; set; }
        public string Temperatura { get; set; }
        public string Ingredientes { get; set; }

        public Cerveja ConverterParaCerveja()
        {
            return ConverterParaCerveja(new Cerveja());
        }

        public Cerveja ConverterParaCerveja(Cerveja cerveja)
        {
            cerveja.Nome = Nome;
            cerveja.Descricao = Descricao;
            cerveja.Harmonizacao = Harmonizacao;
            cerveja.Cor = Cor;
            cerveja.TeorAlcoolico = TeorAlcoolico;
            cerveja.Temperatura = Temperatura;
            cerveja.Ingredientes = Ingredientes;
            return cerveja;
        }
    }
}