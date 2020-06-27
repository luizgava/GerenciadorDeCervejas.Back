using System;
using System.Threading.Tasks;
using GerenciadorDeCervejas.Aplicacao.Infra;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Dominio.Cervejas.DTOs;
using GerenciadorDeCervejas.Dominio.Cervejas.Imagem;
using GerenciadorDeCervejas.Dominio.Infra;
using GerenciadorDeCervejas.Mensageria.Eventos;

namespace GerenciadorDeCervejas.Aplicacao.Cervejas
{
    public class AplicCerveja : AplicBase, IAplicCerveja
    {
        #region ctor
        private readonly IRepCerveja _repCerveja;
        private readonly IRepCervejaImagem _repCervejaImagem;
        private readonly IEventoNotificarAlteracaoCerveja _eventoNotificarAlteracaoCerveja;

        public AplicCerveja(IUnidadeDeTrabalho unidadeDeTrabalho,
                            IRepCerveja repCerveja,
                            IRepCervejaImagem repCervejaImagem, 
                            IEventoNotificarAlteracaoCerveja eventoNotificarAlteracaoCerveja)
            : base(unidadeDeTrabalho)
        {
            _repCerveja = repCerveja;
            _repCervejaImagem = repCervejaImagem;
            _eventoNotificarAlteracaoCerveja = eventoNotificarAlteracaoCerveja;
        }
        #endregion

        #region Inserir
        public async Task<int> Inserir(CervejaDTO dto)
        {
            var obj = dto.ConverterParaCerveja();
            await _repCerveja.InserirAsync(obj);
            await Persistir();
            return obj.Id;
        }
        #endregion

        #region Alterar
        public async Task Alterar(int id, CervejaDTO dto)
        {
            var obj = await _repCerveja.RecuperarPorIdAsync(id);
            dto.ConverterParaCerveja(obj);
            _repCerveja.Alterar(obj);
            await Persistir();

            _eventoNotificarAlteracaoCerveja.Publicar(obj);
        }
        #endregion

        #region Remover
        public async Task Remover(int id)
        {
            _repCerveja.Remover(id);
            await Persistir();
        }
        #endregion

        #region AlterarImagem
        public async Task AlterarImagem(int id, CervejaImagemDTO dto)
        {
            var cerveja = await _repCerveja.RecuperarPorIdAsync(id);
            if (cerveja == null)
            {
                throw new Exception("Cerveja não encontrada.");
            }
            var cervejaImagem = await _repCervejaImagem.RecuperarPorIdAsync(id);
            if (cervejaImagem == null)
            {
                cervejaImagem = new CervejaImagem { Id = id };
                dto.ConverterParaCervejaImagem(cervejaImagem);
                await _repCervejaImagem.InserirAsync(cervejaImagem);
            }
            else
            {
                dto.ConverterParaCervejaImagem(cervejaImagem);
                _repCervejaImagem.Alterar(cervejaImagem);
            }
            _repCerveja.Alterar(cerveja);
            await Persistir();

            _eventoNotificarAlteracaoCerveja.Publicar(cerveja);
        }
        #endregion
    }
}
