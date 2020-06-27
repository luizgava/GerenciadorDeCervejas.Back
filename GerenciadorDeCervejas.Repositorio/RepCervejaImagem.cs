using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Dominio.Cervejas.Imagem;
using GerenciadorDeCervejas.Repositorio.Contexto;
using GerenciadorDeCervejas.Repositorio.Infra;

namespace GerenciadorDeCervejas.Repositorio
{
    public class RepCervejaImagem : RepBase<CervejaImagem>, IRepCervejaImagem
    {
        #region ctor
        public RepCervejaImagem(ContextoBanco contexto)
            : base(contexto)
        {
        }
        #endregion
    }
}
