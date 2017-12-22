using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChamadosPro.Models
{
    public class Chamados
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdChamado{ get; set; }

        public string Descricao { get; set; }
        public int Pa { get; set; }

        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }

        public int IdSubcategoria { get; set; }
        [ForeignKey("IdSubcategoria")]
        public virtual SubCategoria SubCategoria { get; set; }

        public int? IdStatus { get; set; } = 1;
        [ForeignKey("IdStatus")]
        public virtual Status Status { get; set; }

        public int MatriculaOperador { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }
       
        [ForeignKey("UsuarioRequisitante")]
        public string RequisitanteID { get; set; }

        [ForeignKey("UsuarioResponsavel")]
        public string ResponsavelID { get; set; }

        public virtual ApplicationUser UsuarioRequisitante { get; set; }
        public virtual ApplicationUser UsuarioResponsavel { get; set; }

    }
}