using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ChamadosPro.Models
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLog { get; set; }

        public int IdChamado { get; set; } 
        [ForeignKey("IdChamado")]
        public virtual Chamados Chamados { get; set; }

        public string UserName { get; set; }
        [ForeignKey("UserName")]
        public virtual ApplicationUser ResponsavelChamado { get; set; }

        [ForeignKey("UserName")]
        public virtual ApplicationUser RequisitanteChamado { get; set; }



        public DateTime Datalog { get; set; }
        public string Descricao { get; set; }
    }
}