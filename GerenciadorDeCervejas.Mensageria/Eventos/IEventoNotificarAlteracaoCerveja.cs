using GerenciadorDeCervejas.Dominio.Cervejas;

namespace GerenciadorDeCervejas.Mensageria.Eventos
{
    public interface IEventoNotificarAlteracaoCerveja
    {
        void Publicar(Cerveja cerveja);
    }
}