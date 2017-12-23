using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
namespace ChamadosPro.Models
{
    public class Equipamento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IdEquipamento{ get; set; }
    }
}