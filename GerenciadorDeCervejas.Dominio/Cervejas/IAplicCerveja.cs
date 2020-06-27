using System.Threading.Tasks;
using GerenciadorDeCervejas.Dominio.Cervejas.DTOs;

namespace GerenciadorDeCervejas.Dominio.Cervejas
{
    public interface IAplicCerveja
    {
        Task<int> Inserir(CervejaDTO dto);
        Task Alterar(int id, CervejaDTO dto);
        Task Remover(int id);
        Task AlterarImagem(int id, CervejaImagemDTO dto);
    }
}