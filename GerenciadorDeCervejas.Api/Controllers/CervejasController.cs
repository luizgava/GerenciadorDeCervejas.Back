using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GerenciadorDeCervejas.Dominio.Cervejas;
using GerenciadorDeCervejas.Dominio.Cervejas.DTOs;
using GerenciadorDeCervejas.Dominio.Cervejas.Imagem;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorDeCervejas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CervejasController : ControllerBase
    {
        #region ctor
        private readonly IAplicCerveja _aplicCerveja;
        private readonly IRepCerveja _repCerveja;
        private readonly IRepCervejaImagem _repCervejaImagem;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CervejasController(IAplicCerveja aplicCerveja,
                                  IRepCerveja repCerveja, 
                                  IRepCervejaImagem repCervejaImagem, 
                                  IWebHostEnvironment webHostEnvironment)
        {
            _aplicCerveja = aplicCerveja;
            _repCerveja = repCerveja;
            _repCervejaImagem = repCervejaImagem;
            _webHostEnvironment = webHostEnvironment;
        }
        #endregion

        #region Recuperar
        [HttpGet]
        [EnableQuery]
        public async Task<IEnumerable<Cerveja>> Recuperar()
        {
            return await _repCerveja.Recuperar().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Cerveja> RecuperarPorId(int id)
        {
            return await _repCerveja.Recuperar().FirstOrDefaultAsync(x => x.Id == id);
        }
        #endregion

        #region Inserir
        [HttpPost]
        public async Task<IActionResult> Inserir([FromBody] CervejaDTO obj)
        {
            try
            {
                var retorno = await _aplicCerveja.Inserir(obj);
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Alterar
        [HttpPut("{id}")]
        public async Task<IActionResult> Alterar(int id, [FromBody] CervejaDTO dto)
        {
            try
            {
                await _aplicCerveja.Alterar(id, dto);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Remover
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(int id)
        {
            try
            {
                await _aplicCerveja.Remover(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region UploadFoto
        [HttpPut("{id}/UploadFoto")]
        public async Task<IActionResult> UploadFoto(int id, IFormFile formFile)
        {
            try
            {
                if (formFile == null || formFile.Length == 0)
                {
                    return BadRequest("Arquivo não informado.");
                }
                await using var memoryStream = new MemoryStream();
                await formFile.CopyToAsync(memoryStream);
                await _aplicCerveja.AlterarImagem(id, new CervejaImagemDTO
                {
                    NomeArquivo = formFile.FileName,
                    ContentType = formFile.ContentType,
                    Bytes = memoryStream.ToArray()
                });
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region Foto
        [HttpGet("{id}/Foto")]
        public async Task<IActionResult> Foto(int id)
        {
            try
            {
                var imagem = await _repCervejaImagem.RecuperarPorIdAsync(id);
                if (imagem == null)
                {
                    return File(System.IO.File.OpenRead(Path.Combine(_webHostEnvironment.ContentRootPath, "Content", "sem-foto.png")), "image/png");
                }
                return File(new MemoryStream(imagem.Bytes), imagem.ContentType);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}