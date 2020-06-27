using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GerenciadorDeCervejas.Dominio.Infra;
using GerenciadorDeCervejas.Infra.Entidades;
using GerenciadorDeCervejas.Repositorio.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeCervejas.Repositorio.Infra
{
    public class RepBase<T> : IRepBase<T> where T : EntidadeBase
    {
        #region ctor
        private readonly ContextoBanco _contexto;
        private DbSet<T> DbSet => _contexto.Set<T>();

        public RepBase(ContextoBanco contexto)
        {
            _contexto = contexto;
        }
        #endregion

        #region Recuperar
        public IQueryable<T> Recuperar()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<T> Recuperar(Expression<Func<T, bool>> predicado)
        {
            return DbSet.Where(predicado);
        }
        #endregion

        #region RecuperarPorIdAsync
        public async Task<T> RecuperarPorIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }
        #endregion

        #region InserirAsync
        public async Task InserirAsync(T entidade)
        {
            await DbSet.AddAsync(entidade);
        }
        #endregion

        #region Alterar
        public void Alterar(T entidade)
        {
            DbSet.Update(entidade);
        }
        #endregion

        #region Remover
        public void Remover(int id)
        {
            var entidade = DbSet.Find(id);
            DbSet.Remove(entidade);
        }
        #endregion
    }
}