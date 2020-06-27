using System;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using GerenciadorDeCervejas.Aplicacao.Cervejas;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Dominio.Cervejas.DTOs;
using GerenciadorDeCervejas.Dominio.Cervejas.Imagem;
using GerenciadorDeCervejas.Dominio.Infra;
using GerenciadorDeCervejas.Mensageria.Eventos;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GerenciadorDeCervejas.Aplicacao.Teste
{
    [TestClass]
    public class TesteAplicCerveja
    {
        #region ctor
        private readonly IUnidadeDeTrabalho _unidadeDeTrabalho;
        private readonly IRepCerveja _repCerveja;
        private readonly IRepCervejaImagem _repCervejaImagem;
        private readonly IEventoNotificarAlteracaoCerveja _eventoNotificarAlteracaoCerveja;
        private readonly AplicCerveja _aplicCerveja;

        public TesteAplicCerveja()
        {
            _unidadeDeTrabalho = A.Fake<IUnidadeDeTrabalho>();
            _repCerveja = A.Fake<IRepCerveja>();
            _repCervejaImagem = A.Fake<IRepCervejaImagem>();
            _eventoNotificarAlteracaoCerveja = A.Fake<IEventoNotificarAlteracaoCerveja>();
            _aplicCerveja = new AplicCerveja(_unidadeDeTrabalho, 
                                             _repCerveja,
                                             _repCervejaImagem,
                                             _eventoNotificarAlteracaoCerveja);
        }
        #endregion

        #region Inserir
        [TestMethod]
        public async Task Inserir_deve_chamar_repositorio_e_comittar()
        {
            var dto = new CervejaDTO { Nome = "Corona" };
            await _aplicCerveja.Inserir(dto);

            A.CallTo(() => _repCerveja.InserirAsync(A<Cerveja>._)).MustHaveHappened();
            A.CallTo(() => _unidadeDeTrabalho.Persistir()).MustHaveHappened();
        }
        #endregion

        #region Alterar
        [TestMethod]
        public async Task Alterar_deve_chamar_repositorio_comittar_e_enviar_msg_rabbit()
        {
            var id = 1;
            var dto = new CervejaDTO { Nome = "Corona" };
            await _aplicCerveja.Alterar(id, dto);

            A.CallTo(() => _repCerveja.Alterar(A<Cerveja>._)).MustHaveHappened();
            A.CallTo(() => _unidadeDeTrabalho.Persistir()).MustHaveHappened();
            A.CallTo(() => _eventoNotificarAlteracaoCerveja.Publicar(A<Cerveja>._)).MustHaveHappened();
        }
        #endregion

        #region Remover
        [TestMethod]
        public async Task Remover_deve_chamar_repositorio_e_comittar()
        {
            var id = 1;
            await _aplicCerveja.Remover(id);

            A.CallTo(() => _repCerveja.Remover(id)).MustHaveHappened();
            A.CallTo(() => _unidadeDeTrabalho.Persistir()).MustHaveHappened();
        }
        #endregion

        #region AlterarImagem
        [TestMethod]
        public void AlterarImagem_passando_cerveja_que_nao_existe_deve_dar_excecao()
        {
            var id = 1;
            var dto = new CervejaImagemDTO();
            A.CallTo(() => _repCerveja.RecuperarPorIdAsync(id)).Returns((Cerveja)null);

            _aplicCerveja.Invoking(x => x.AlterarImagem(id, dto)).Should().Throw<Exception>().WithMessage("Cerveja não encontrada.");
        }

        [TestMethod]
        public async Task AlterarImagem_passando_cerveja_que_nao_tem_imagem_deve_inserir_e_chamar_rabbit()
        {
            var id = 1;
            var dto = new CervejaImagemDTO();
            var cerveja = new Cerveja();
            A.CallTo(() => _repCerveja.RecuperarPorIdAsync(id)).Returns(cerveja);
            A.CallTo(() => _repCervejaImagem.RecuperarPorIdAsync(id)).Returns((CervejaImagem) null);

            await _aplicCerveja.AlterarImagem(id, dto);
            
            A.CallTo(() => _repCervejaImagem.InserirAsync(A<CervejaImagem>._)).MustHaveHappened();
            A.CallTo(() => _unidadeDeTrabalho.Persistir()).MustHaveHappened();
            A.CallTo(() => _eventoNotificarAlteracaoCerveja.Publicar(A<Cerveja>._)).MustHaveHappened();
        }

        [TestMethod]
        public async Task AlterarImagem_passando_cerveja_que_tem_imagem_deve_alterar_e_chamar_rabbit()
        {
            var id = 1;
            var dto = new CervejaImagemDTO();
            var cerveja = new Cerveja();
            A.CallTo(() => _repCerveja.RecuperarPorIdAsync(id)).Returns(cerveja);
            var cervejaImagem = new CervejaImagem();
            A.CallTo(() => _repCervejaImagem.RecuperarPorIdAsync(id)).Returns(cervejaImagem);
            
            await _aplicCerveja.AlterarImagem(id, dto);

            A.CallTo(() => _repCervejaImagem.Alterar(A<CervejaImagem>._)).MustHaveHappened();
            A.CallTo(() => _unidadeDeTrabalho.Persistir()).MustHaveHappened();
            A.CallTo(() => _eventoNotificarAlteracaoCerveja.Publicar(A<Cerveja>._)).MustHaveHappened();
        }
        #endregion
    }
}
