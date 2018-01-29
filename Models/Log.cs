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
        [Display(Name = "Log")]
        public int IdLog { get; set; }

        [Display(Name = "Ticket Chamado")]
        public int IdChamado { get; set; } 
        [ForeignKey("IdChamado")]
        public virtual Chamados Chamados { get; set; }

        [Display(Name = "Usuario")]
        [ForeignKey("UsuarioResponsavel")]
        public string ResponsavelID { get; set; }
        
        public virtual ApplicationUser UsuarioResponsavel { get; set; }

        [Display(Name = "Data da interação")]
        public DateTime Datalog { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }
}