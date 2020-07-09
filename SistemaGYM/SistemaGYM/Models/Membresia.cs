using SistemaGYM.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGYM.Models
{
    public class Membresia
    {
        public ApplicationDbContext _context { get; set; }

        public Membresia() { }
        public Membresia(ApplicationDbContext context)
        {
            _context = context;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MembresiaID { get; set; }
        [ForeignKey("SocioID")]
        public int SocioID { get; set; }
        [Required(ErrorMessage = "Debes ingresar un valor.")]
        [Display(Name = "Costo de la membresia")]
        public float Cost { get; set; }
        [Required(ErrorMessage = "Se requiere una fecha")]
        [Display(Name = "Fecha de inicio")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Se requie ingresar un numero de meses mayor a 1 y menor a 12")]
        [Display(Name = "Meses de duracion")]
        public int ExpiraEn { get; set; }
        


        public DateTime FinishDate()
        {
            return StartDate.AddMonths(ExpiraEn);
        }

        public string SocioName()
        {
           return  _context.Socio.Where(x => x.SocioID == SocioID).FirstOrDefault().FullName;
        }

        public bool CheckState()
        {
            DateTime Today = DateTime.Now;

            if (DateTime.Compare(FinishDate(), Today) == 0 || DateTime.Compare(FinishDate(), Today) < 0)
                return false;
            else 
                return true;
        }

        public string Row()
        {
            string Estado;

            if (CheckState())
                Estado = "table-success";
            else
                Estado = "table-danger";

            return "<tr class='" + Estado + "'>" +
                    "<td>" + SocioName() + "</td>" +
                    "<td>" + Cost + "</td>" +
                    "<td>" + StartDate.ToLongDateString() + "</td>" +
                    "<td>" + FinishDate().ToLongDateString() + "</td>" +
                    "<td>" + "<a class='btn btn-warning' data-target='#mEliminar' data-toggle='modal' onclick='Membresias.SaveData(\"" + MembresiaID + "\")'>Eliminar</a>" + "</td>" +
                    "</tr>";

        }

    }
}
