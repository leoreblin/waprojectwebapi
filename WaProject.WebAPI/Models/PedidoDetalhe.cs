using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaProject.WebAPI.Models
{
    public class PedidoDetalhe
    {
        [Key]
        public long PedidoDetalheId { get; set; }

        public long PedidoId { get; set; }

        public long ProdutoId { get; set; }
        public Produto Produto { get; set; }

        public int Quantidade { get; set; }

        public decimal ProdutoItemValor { get; set; }
    }
}
