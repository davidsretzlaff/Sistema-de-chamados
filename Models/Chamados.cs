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
        [Display(Name = "Ticket")]
        public int IdChamado { get; set; }

        [Display(Name = "Descrição")]
        [Required]
        public string Descricao { get; set; }

        public int Pa { get; set; }

        [Display(Name = "Categoria")]
        public int IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }

        [Display(Name = "Subcategoria")]
        public int IdSubcategoria { get; set; }
        [ForeignKey("IdSubcategoria")]
        public virtual SubCategoria SubCategoria { get; set; }

        [Display(Name = "Statusx")]
        public int? IdStatus { get; set; } = 1;
        [ForeignKey("IdStatus")]
        public virtual Status Status { get; set; }

        [Display(Name = "Operador")]
        public int MatriculaOperador { get; set; }

        [Display(Name = "Data de abertura")]
        public DateTime DataAbertura { get; set; }

        [Display(Name = "Data de fechamento")]
        public DateTime? DataFechamento { get; set; }

        [Display(Name = "Requisitante")]
        [ForeignKey("UsuarioRequisitante")]
        public string RequisitanteID { get; set; }


        [ForeignKey("UsuarioResponsavel")]
        [Display(Name = "Responsável")]
        public string ResponsavelID { get; set; }

        [Display(Name = "Equipamento")]
        [ForeignKey("Equipamento")]
        public string EquipamentoID { get; set; }

        public virtual Equipamento Equipamento { get; set; }
        public virtual ApplicationUser UsuarioRequisitante { get; set; }
        [Display(Name = "Responsavel")]
        public virtual ApplicationUser UsuarioResponsavel { get; set; }

    }
}
