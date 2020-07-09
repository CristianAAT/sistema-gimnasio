using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGYM.Models
{
    public class Entrada
    {
        public Entrada() {
        
            Types = new List<SelectListItem>
                {
                    new SelectListItem{ Value = "Visitante", Text = "Visitante" },
                    new SelectListItem{ Value = "Socio", Text = "Socio"}
                };
        
        }


        //Attributes and methods
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EntradaID { get; set; }
        [Display(Name = "Fecha de hoy")]
        public DateTime DayDate { get; set; }
        [Display(Name = "Tipo de entrada")]
        public String Type { get; set; }
        [Display(Name = "Costo de la visita")]
        public float VisitCost { get; set; }

        [NotMapped]
        public List<SelectListItem> Types { get; set; }

        public string Row()
        {
        return "<tr>" +
                    "<td>" + DayDate.ToLongDateString() + "</td>" +
                    "<td>" + Type + "</td>" +
                    "<td>" + VisitCost + "</td>" +
                    "<td>" + "<a class='btn btn-warning' data-target='#mEliminar' data-toggle='modal' onclick='Entradas.SaveData(\"" + EntradaID + "\")'>Eliminar</a>" + "</td>" +
               "</tr>";
        }

    }
}
