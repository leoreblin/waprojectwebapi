using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaProject.WebAPI.Models
{
    public class Equipe
    {
        [Key]
        public int EquipeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Nome { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Descricao { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string PlacaVeiculo { get; set; }
    }
}