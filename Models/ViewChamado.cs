using System;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDViewChamado{ get; set; }
        public List<Chamados> Chamado { get; set; }
        public List<Log> Log { get; set; }
        public List<Equipamento> Equipamento { get; set; }
    }
}