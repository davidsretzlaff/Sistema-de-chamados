using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
namespace ChamadosPro.Models
{
    public class Status
    {
        [Display(Name = "Status")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdStatus { get; set; }

        [Display(Name = "Status")]
        public string Descricao { get; set; }

    }
}