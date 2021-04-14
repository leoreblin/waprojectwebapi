using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WaProject.WebAPI.Context;

namespace WaProject.WebAPI.Models
{
    public class Encomenda
    {
        public long PedidoId { get; set; }
        public Equipe Equipe { get; set; }
    }
}
