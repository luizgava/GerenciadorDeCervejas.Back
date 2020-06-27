using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Repositorio.Contexto;
using GerenciadorDeCervejas.Repositorio.Infra;

namespace GerenciadorDeCervejas.Repositorio
{
    public class RepCerveja : RepBase<Cerveja>, IRepCerveja
    {
        #region ctor
        public RepCerveja(ContextoBanco contexto)
            : base(contexto)
        {
        }
        #endregion
    }
}
