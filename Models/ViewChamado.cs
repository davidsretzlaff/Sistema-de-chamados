using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChamadosPro.Models
{
    public class ViewChamado 
    {
        [Key]
        public int IdViewChamado{ get; set; }
        public Chamados Chamado { get; set; }
        public IEnumerable<Log> Log { get; set; }
        public Equipamento Equipamento { get; set; }

    }
}