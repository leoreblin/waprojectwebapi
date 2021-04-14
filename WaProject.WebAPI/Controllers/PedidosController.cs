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

            return pedidosPaginados;
        }

        // GET: v1/Pedidos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedido(long id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();
            
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
                return BadRequest("O objeto patchDocument é null");
            }
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(pedido, ModelState);
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        // PUT: v1/Pedidos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(long id, [FromBody] Pedido pedido)
        {
            if (id != pedido.PedidoId)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }

            return NoContent();
        }
		
        // POST: v1/Pedidos
        [HttpPost]
        public async Task<ActionResult<Pedido>> PostPedido([FromBody] Pedido pedido)
        {
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
