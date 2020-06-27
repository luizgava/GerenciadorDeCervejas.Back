using GerenciadorDeCervejas.Infra.Entidades;

namespace GerenciadorDeCervejas.Dominio.Cervejas.Imagem
{
    public class CervejaImagem : EntidadeBase
    {
        public string NomeArquivo { get; set; }
        public string ContentType { get; set; }
        public byte[] Bytes { get; set; }

        public Cerveja Cerveja { get; set; }
    }
}