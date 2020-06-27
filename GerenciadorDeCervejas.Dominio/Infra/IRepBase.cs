using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GerenciadorDeCervejas.Infra.Entidades;

namespace GerenciadorDeCervejas.Dominio.Infra
{
    public interface IRepBase<T> where T : EntidadeBase
    {
        IQueryable<T> Recuperar();
        IQueryable<T> Recuperar(Expression<Func<T, bool>> predicado);
        Task<T> RecuperarPorIdAsync(int id);
        Task InserirAsync(T entidade);
        void Alterar(T entidade);
        void Remover(int id);
    }
}