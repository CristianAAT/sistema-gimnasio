using Microsoft.AspNetCore.Identity;
using SistemaGYM.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaGYM.Models
{
    public class UsuariosAPP : IdentityUser
    {
        private ApplicationDbContext _context;
        public List<IdentityUser> UserList { get; set; }

        public UsuariosAPP()
        {
//contructor sin parametros para enviar un modelo limpio al controlador
        }

        public UsuariosAPP(ApplicationDbContext context)
        {
            _context = context;
            UserList = _context.Users.ToList();
            Roles = new List<SelectListItem> { 
                new SelectListItem{ Value = "Admin", Text = "Administrador del sistema (super usuario)" },
                new SelectListItem{ Value = "Gerente", Text = "Gerente del sistema (empleado encargado del sistema)" },
                new SelectListItem{ Value = "Miembro", Text = "Miembro (usuario comun)" }
            };
        }

        [ProtectedPersonalData]
        [Required(ErrorMessage = "El campo de Correo es obligatorio")]
        [EmailAddress (ErrorMessage = "Ingresa una direccion de correo valida")]
        [Display(Name = "Correo")]
        public override string Email { get; set; }

        [Required (ErrorMessage = "Ingresar una contraseña es obligatorio")]
        [StringLength(100, ErrorMessage = "La {0} debe tener almenos {2} y un maximo de {1} caracteres.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,25}$", ErrorMessage = "La contraseña debe tener almenos: Un numero, una letra minuscula, una letra mayuscula y un caracter no alfanumerico")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La confirmacion no coincide con la contraseña.")]
        public string ConfirmPassword { get; set; }

        [StringLength(20, ErrorMessage = "El {0} debe tener almenos {2} y un maximo de {1} caracteres.", MinimumLength = 5)]
        [Display(Name = "Nombre de usuario")]
        public override string UserName { get; set; }
        
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Numero de telefono invalido")]
        [Display(Name = "Numero de telefono")]
        public override string PhoneNumber { get; set; }

        public string Rol { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public string Row()
        {
            return "<tr>" +
                    "<td>" + UserName + "</td>" +
                    "<td>" + Email + "</td>" +
                    "<td>" + Rol + "</td>" +
                    "<td>" + "<a class='btn btn-outline-info' data-target='#mEditar' data-toggle='modal' onclick='Usuarios.GetUsuario(\"" + Id + "\")'>Información</a>" + "</td>" +
                    "<td>" + "<a class='btn btn-outline-danger' data-target='#mEliminar' data-toggle='modal' onclick='Usuarios.SaveData(\"" + Id + "\")'>Eliminar</a>" + "</td>" +
                    "</tr>";

        }

    }
}
