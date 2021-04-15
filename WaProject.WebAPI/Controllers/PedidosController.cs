using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaProject.WebAPI.Context;
using WaProject.WebAPI.Filters;
using WaProject.WebAPI.Helpers;
using WaProject.WebAPI.Models;
using WaProject.WebAPI.Responses;
using WaProject.WebAPI.Services;

namespace WaProject.WebAPI.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class PedidosController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IUriService _uriService;

        public PedidosController(ApiContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        // GET: v1/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos([FromQuery] PaginationFilter filter)
        {
            var filtro = new PaginationFilter(filter.PageNumber, filter.PageSize);
            string route = Request.Path.Value;
            var pedidosPaginados = await _context.Pedidos
                .AsNoTracking()
                .OrderByDescending(p => p.DtCriacao)
                .Skip((filtro.PageNumber - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .ToListAsync();

            foreach (var pedido in pedidosPaginados)
            {
                pedido.Itens = new List<PedidoDetalhe>();
                pedido.Equipe ??= await _context.Equipes.FindAsync(pedido.EquipeId);
                List<PedidoDetalhe> itens = await _context.PedidoDetalhes.AsNoTracking().Where(x => x.PedidoId == pedido.PedidoId).ToListAsync();
                foreach (var item in itens)
                {
                    item.Produto ??= await _context.Produtos.FindAsync(item.ProdutoId);
                    pedido.Itens.Add(item);
                }
            }

            var totalPedidos = _context.Pedidos.Count();
            var response = PaginationHelper.CreatePagedReponse<Pedido>(pedidosPaginados.ToList(), filtro, totalPedidos, _uriService, route);

            return Ok(new CustomResponse<List<Pedido>>(pedidosPaginados, false, "sucesso", new OkResult().StatusCode));
        }

        // GET: v1/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(long id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound(new CustomResponse<Pedido>(null, true, "Não foi encontrado pedido com o Id informado.", new NotFoundResult().StatusCode));
            }                
            
            pedido.Equipe ??= await _context.Equipes.FindAsync(pedido.EquipeId);

            foreach (var item in pedido.Itens)
            {
                item.Produto ??= await _context.Produtos.FindAsync(item.ProdutoId);
            }

            return pedido;
        }
		
        [HttpPatch("atualizar-campos/{id}")]
        public async Task<ActionResult> PatchPedido(long id, [FromBody] JsonPatchDocument<Pedido> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest(new CustomResponse<Pedido>(null, true, "O objeto patchDocument não pode ser null", new BadRequestResult().StatusCode));
            }
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound(new CustomResponse<Pedido>(null, true, "Não foi encontrado pedido com o Id informado.", new NotFoundResult().StatusCode)); ;
            }

            patchDocument.ApplyTo(pedido, ModelState);
            await _context.SaveChangesAsync();

            return Ok(new CustomResponse<Pedido>(pedido, false, "sucesso", new OkResult().StatusCode));
        }

        // PUT: v1/Pedidos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(long id, [FromBody] Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return BadRequest(new CustomResponse<Pedido>(null, true, "O Id do pedido informado não corresponde ao Id do objeto Pedido", new BadRequestResult().StatusCode));
            }

            _context.Entry(pedido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PedidoExists(id))
                {
                    return NotFound(new CustomResponse<Pedido>(null, true, "Não foi encontrado pedido com o Id informado.", new NotFoundResult().StatusCode));
                }
                else
                {
                    return new StatusCodeResult(500);
                }
            }

            return Ok(new CustomResponse<Pedido>(await _context.Pedidos.FindAsync(id) ,false, "sucesso", new OkResult().StatusCode));
        }
		
        // POST: v1/Pedidos
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido([FromBody] Pedido pedido)
        {
            if (pedido.Itens.Count == 0)
            {
                return NotFound(new CustomResponse<Pedido>(null, true, "Nenhum item foi inserido no pedido.", new NotFoundResult().StatusCode)); 
            }
            else
            {
                List<string> itemIndexes = new List<string>();
                int i = 0;
                foreach (var item in pedido.Itens)
                {
                    i++;
                    if (item.ProdutoId == 0 || await _context.Produtos.FindAsync(item.ProdutoId) == null)
                    {
                        itemIndexes.Add(i.ToString("00"));
                    }
                }

                if (itemIndexes.Count > 0)
                {
                    return NotFound(
                        new CustomResponse<Pedido>(
                            null,
                            true,
                            "Não foi / foram encontrado(s) ou não foi / foram informado(s) o(s) Produto(s) no(s) Item(ns) " + string.Join(",", itemIndexes) + " solicitado(s).",
                            new NotFoundResult().StatusCode
                            )
                        );
                }
            }

            pedido.DtCriacao = DateTime.Now;
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPedido", new { id = pedido.PedidoId }, pedido);
        }
		
        // DELETE: v1/Pedidos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Pedido>> DeletePedido(long id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return pedido;
        }

        private bool PedidoExists(long id)
        {
            return _context.Pedidos.Any(e => e.PedidoId == id);
        }
    }
}
