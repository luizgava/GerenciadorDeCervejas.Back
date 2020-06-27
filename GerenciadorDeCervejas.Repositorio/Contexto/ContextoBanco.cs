using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Infra.Entidades;
using GerenciadorDeCervejas.Repositorio.Configurações;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeCervejas.Repositorio.Contexto
{
    public class ContextoBanco : DbContext
    {
        #region ctor
        public ContextoBanco()
        {

        }

        public ContextoBanco(DbContextOptions<ContextoBanco> options)
            : base(options)
        {

        }
        #endregion

        #region SaveChanges
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            AtualizarDatasCriacaoEAlteracao();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            AtualizarDatasCriacaoEAlteracao();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges()
        {
            AtualizarDatasCriacaoEAlteracao();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AtualizarDatasCriacaoEAlteracao();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void AtualizarDatasCriacaoEAlteracao()
        {
            var registrosModificados = from p in ChangeTracker.Entries()
                                      where (p.State == EntityState.Added || p.State == EntityState.Modified)
                                         && p.Entity is EntidadeBase
                                     select new
                                     {
                                         p.State,
                                         Entidade = p.Entity as EntidadeBase
                                     };
            foreach (var item in registrosModificados)
            {
                if (item.State == EntityState.Added)
                {
                    item.Entidade.DataCriacao = DateTime.Now;
                }
                item.Entidade.DataAlteracao = DateTime.Now;
            }
        }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CervejaConfiguration());
            modelBuilder.ApplyConfiguration(new CervejaImagemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}