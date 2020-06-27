using System;
using System.Threading.Tasks;

namespace GerenciadorDeCervejas.Dominio.Infra
{
    public interface IUnidadeDeTrabalho : IDisposable
    {
        Task Persistir();
        void Rejeitar();
    }
}