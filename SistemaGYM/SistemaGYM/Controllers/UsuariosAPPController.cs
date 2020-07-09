using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGYM.Data;
using SistemaGYM.GenericClass;
using SistemaGYM.Models;

namespace SistemaGYM.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosAPPController : Controller
    {
        public UsuariosAPP model;
        public IdentityError Respuesta;
        public UserManager<IdentityUser> _userManager;
        public ApplicationDbContext _context;

        public UsuariosAPPController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;

        }

        public IActionResult UsuariosAPP()
        {

            model = new UsuariosAPP(_context);
            return View(model);
        }
        
        [HttpPost] //Este metodo requiere de un contructor de UsuariosAPP para instanciar esta clase(UsuariosAPP) sin contexto.
        public async Task<string> Agregar(UsuariosAPP model)
        {
            
            Respuesta = new IdentityError();

            var usuario = new IdentityUser
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = true
            };

            //validaciones
            if (!usuario.Email.Contains('@') || !usuario.Email.Contains('.'))
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "* Agrega una direccion de correo valida";
                return JsonConvert.SerializeObject(Respuesta);
            }
            else if (_userManager.FindByEmailAsync(usuario.Email).Result != null)
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "Ya existe un usuario con ese correo registrado.";
                return JsonConvert.SerializeObject(Respuesta);
            }
            else if (_userManager.FindByNameAsync(usuario.UserName).Result != null)
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "Ese nombre de usuario ya esta ocupado.";
                return JsonConvert.SerializeObject(Respuesta);
            }

            try
            {
                IdentityResult Result = await _userManager.CreateAsync(usuario, model.Password);

                if (Result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuario, model.Rol);
                    
                    Respuesta.Code = "OK";
                }
                else
                {
                    Respuesta.Code = "ERROR";
                    foreach (var error in Result.Errors)
                    {
                        Respuesta.Description += error.Description + "\n";
                    }
                }

                
                return JsonConvert.SerializeObject(Respuesta);
            }
            catch (Exception ex)
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = ex.ToString();
                return JsonConvert.SerializeObject(Respuesta);
            }
        }

        [HttpPost]
        public async Task<string> Eliminar(string Id)
        {
            Respuesta = new IdentityError();

            try
            {
                var UserLogin = await _userManager.GetUserAsync(HttpContext.User);
                var user = await _userManager.FindByIdAsync(Id);

                IdentityResult Result;

                

                if (UserLogin == user)
                {
                    Result = new IdentityResult ();
                    Respuesta.Description = "No te puedes eliminar a ti mismo.";
                }
                else
                {
                    Result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                    Result = await _userManager.DeleteAsync(user);
                }



                if (Result.Succeeded)
                {
                    
                    Respuesta.Code = "OK";
                    Respuesta.Description = "Eliminado";
                    return JsonConvert.SerializeObject(Respuesta);
                }
                else
                {
                    Respuesta.Code = "ERROR";
                    foreach (var error in Result.Errors)
                    {
                        Respuesta.Description += error + " \n";
                    }
                    return JsonConvert.SerializeObject(Respuesta);
                }
            }
            catch (Exception ex)
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = ex.ToString();
                return JsonConvert.SerializeObject(Respuesta);
            }
        }

        [HttpPost]
        public async Task<string> Editar(UsuariosAPP model)
        {
            Respuesta = new IdentityError();

            var User = await _userManager.FindByEmailAsync(model.Email);
            var Rol = await _userManager.GetRolesAsync(User);
            //actualizacion de datos
            User.UserName = model.UserName;
            User.PhoneNumber = model.PhoneNumber;

            try
            {
                var Result = await _userManager.UpdateAsync(User);
                if (model.Rol != Rol.ElementAt(0))
                {
                    await _userManager.RemoveFromRolesAsync(User, Rol);
                    await _userManager.AddToRoleAsync(User, model.Rol);
                }

                if (Result.Succeeded)
                    Respuesta.Code = "OK";
                else
                {
                    Respuesta.Code = "ERROR";
                    foreach (var error in Result.Errors)
                    {
                        Respuesta.Description += error.Description + "\n";
                    }
                }


                return JsonConvert.SerializeObject(Respuesta);

            }
            catch (Exception Ex)
            {

                Respuesta.Code = "ERROR";
                Respuesta.Description = Ex.ToString();
                return JsonConvert.SerializeObject(Respuesta);
            }

        }

        [HttpGet]
        public async Task<string> GetUsuario(string Id)
        {
            var _User = await _userManager.FindByIdAsync(Id);
            var Rol = await _userManager.GetRolesAsync(_User);
            UsuariosAPP User = new UsuariosAPP { 
                Email = _User.Email,
                UserName = _User.UserName,
                PhoneNumber = _User.PhoneNumber,
                Rol = Rol.ElementAt(0)
            };
            return JsonConvert.SerializeObject(User);
        }

        [HttpPost]
        public List<string> Filtrar(int Pagina, int Registros, string Value)
        {
            Object[] Objects = new Object[3];
            var data = SearchUsuarios(Value);
            List<String> Retorno = new List<string>();

            if (data.Count > 0)
            {
                Objects = new Paginador<UsuariosAPP>().paginar(data, Pagina, Registros, "Usuarios");

                //sacar un solo string de los datos
                String FinalData = "";
                foreach (var item in (List<UsuariosAPP>)Objects[2])
                {
                    FinalData += item.Row();
                }

                Objects[2] = FinalData;
            }
            else
            {
                Objects[0] = "No hay datos para mostrar";
                Objects[1] = "No hay datos para mostrar";
                Objects[2] = "No hay datos para mostrar";
            }

            Retorno.Add((String)Objects[0]);
            Retorno.Add((String)Objects[1]);
            Retorno.Add((String)Objects[2]);

            return Retorno;

        }

        private List<UsuariosAPP> SearchUsuarios(string value)
        {
            List<IdentityUser> lista;
            List<UsuariosAPP> retorno = new List<UsuariosAPP>();

            if (value == "null" || value == null)
            {
                lista = _userManager.Users.ToList();
            }
            else
            {
                lista = _userManager.Users.Where(c => c.UserName.StartsWith(value)).ToList();
            }

            foreach (var item in lista)
            {

                var RolList = Task.Run(async () => await _userManager.GetRolesAsync(item)).ConfigureAwait(false).GetAwaiter().GetResult(); // sacar rol del usuario, obtiene una lista pero sabemos que los usuarios en este caso solo tienen un rol, no hace falta manejar la lista


                UsuariosAPP aux = new UsuariosAPP
                {
                    Id = item.Id,
                    Email = item.Email,
                    UserName = item.UserName,
                    PhoneNumber = item.PhoneNumber,
                    Rol = RolList.ElementAt(0)
                   
                };

                
                retorno.Add(aux);
            }


            return retorno;
        }
    }
}