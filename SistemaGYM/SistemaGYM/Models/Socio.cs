using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGYM.Models
{
    //La clase participa como la inscripcciones al sistema
    public class Socio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SocioID { get; set; }
        [ForeignKey("UsuarioAPPID")]
        public int? UsuarioAPPID { get; set; }
        [ForeignKey("MembresiaID")]
        public int? MembresiaID { get; set; }

        [Required(ErrorMessage = "El campo de nombre debe ser completado")]
        [StringLength(75, ErrorMessage = "El nombre completo debe ser mayor a 20 caracteres y menor a 100", MinimumLength = 20)]
        public string FullName { get; set; }
        public string Email { get; set; }

        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Numero de telefono invalido")]
        public string PhoneNumber { get; set; }
        public string PictureAvatar { get; set; }

        
        public DateTime BirthDate { get; set; }
        public DateTime InscriptionDate { get; set; }
        public DateTime InscriptionExpiration { get; set; }
        public float InscriptionCost { get; set; }
        public bool State { get; set; }


        public string Card()
        {
            string Estado;

            if (State)
                Estado = "border-success";
            else
                Estado = "border-danger";

            return "<div class='col-md-3 col-sm-6 col-xs-8 space-top-sm'>"+
                        "<div class='card card-body "+ Estado +"'>"+
                            "<img class='card-img-top img-fluid rounded-circle' src='" + PictureAvatar + "' data-holder-rendered='true' style='height: 180px; width: 100%; display: block;' />" +
                            "<div class'card-body'>"+
                                "<h5 class'card-title'>" + FullName + "</h5>"+
                                "<p>" + Email +"</p>"+
                                "<a class='btn btn-outline-info' data-target='#mEditar' data-toggle='modal' onclick='Inscripciones.GetSocio(\"" + SocioID + "\")'>Información</a>" +
                                "<a class='btn btn-outline-danger' data-target='#mEliminar' data-toggle='modal' onclick='Inscripciones.SaveData(\"" + SocioID + "\")'>Eliminar</a>" +
                            "</div>" +
                        "</div>"+
                    "</div>";
        }

    }
}
