using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
namespace ChamadosPro.Models
{
    public class SubCategoria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdSubcategoria { get; set; }

        [Display(Name = "Sub-Categoria")]
        public string Descricao { get; set; }

        [Display(Name = "Categoria")]
        public int? IdCategoria { get; set; }
        [ForeignKey("IdCategoria")]
        public virtual Categoria Categoria { get; set; }

    }
}