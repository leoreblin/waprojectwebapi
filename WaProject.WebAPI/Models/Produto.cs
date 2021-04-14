using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WaProject.WebAPI.Models
{
    public class Produto
    {
        [Key]
        public long ProdutoId { get; set; }

        [MinLength(3, ErrorMessage = "Este campo deve conter no mínimo caracteres.")]
        [MaxLength(255, ErrorMessage = "Este campo deve conter no máximo 255 caracteres.")]
        [Display(Name = "Nome do produto")]
        public string Nome { get; set; }

        [MaxLength(255, ErrorMessage = "Este campo deve conter no máximo 255 caracteres.")]
        public string Descricao { get; set; }

        public decimal Valor { get; set; }
    }
}
