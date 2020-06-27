using System.Linq;
using System.Threading.Tasks;
using GerenciadorDeCervejas.Dominio.Infra;
using GerenciadorDeCervejas.Repositorio.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeCervejas.Repositorio.Infra
{
    public class UnidadeDeTrabalho : IUnidadeDeTrabalho
    {
        #region ctor
        private readonly ContextoBanco _contexto;

        public UnidadeDeTrabalho(ContextoBanco contexto)
        {
            _contexto = contexto;
        }
        #endregion

        #region Persistir
        public async Task Persistir()
        {
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch
            {
                Rejeitar();
                throw;
            }
        }
        #endregion

        #region Rejeitar
        public void Rejeitar()
        {
            var changedEntries = _contexto.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();
            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            _contexto.Dispose();
        }
        #endregion
    }
}
