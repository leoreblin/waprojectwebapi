using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaProject.WebAPI.Models
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        public long PedidoId { get; set; }

        public int EquipeId { get; set; }
        public Equipe Equipe { get; set; }

        public List<PedidoDetalhe> Itens { get; set; }

        [Display(Name = "Data e hora da criação do pedido")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime DtCriacao { get; set; }

        [Display(Name = "Data e hora da entrega do pedido")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DtEntregaRealizada { get; set; }

        [Required(ErrorMessage = "Informe o endereço de entrega do pedido")]
        [StringLength(100)]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public decimal Valor { get; set; }      

    }
}
