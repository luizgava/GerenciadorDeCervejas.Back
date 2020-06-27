using System.Threading.Tasks;
using GerenciadorDeCervejas.Dominio.Infra;

namespace GerenciadorDeCervejas.Aplicacao.Infra
{
    public class AplicBase
    {
        #region ctor
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;

        public AplicBase(IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            _unidadeDeTrabalho = unidadeDeTrabalho;
        }
        #endregion

        #region Persistir
        protected async Task Persistir()
        {
            await _unidadeDeTrabalho.Persistir();
        }
        #endregion

        #region Rejeitar
        protected void Rejeitar()
        {
            _unidadeDeTrabalho.Rejeitar();
        }
        #endregion
    }
}