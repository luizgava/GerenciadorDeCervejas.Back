using GerenciadorDeCervejas.Dominio.Cervejas.Imagem;

namespace GerenciadorDeCervejas.Dominio.Cervejas.DTOs
{
    public class CervejaImagemDTO
    {
        public string NomeArquivo { get; set; }
        public string ContentType { get; set; }
        public byte[] Bytes { get; set; }

        public CervejaImagem ConverterParaCervejaImagem(CervejaImagem obj)
        {
            obj.NomeArquivo = NomeArquivo;
            obj.ContentType = ContentType;
            obj.Bytes = Bytes;
            return obj;
        }
    }
}