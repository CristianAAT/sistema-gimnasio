using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaGYM.Data;
using SistemaGYM.Models;

namespace SistemaGYM.Controllers
{
    [Authorize(Roles = "Admin, Gerente")]
    public class InscripcionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Socio model;
        private IdentityError Respuesta;
        private IWebHostEnvironment Environment;

        public InscripcionesController(ApplicationDbContext context, IWebHostEnvironment Environment)
        {
            _context = context;
            this.Environment = Environment;
        }

        public IActionResult Socios()
        {
            model = new Socio();
            return View(model);
        }


        [HttpPost]
        public String Imagen(IFormFile file)
        {
            string avatar = "";
                try
                {
                    //insertar foto
                    var path = Environment.WebRootPath + "\\Inscripciones\\";
                    if (file != null)
                    {
                        path = path + "NewSocio" + ".jpg";

                        using (var stream = System.IO.File.Create(path))
                        {
                            file.CopyTo(stream);
                            stream.Flush();
                        }
                         avatar = "NewSocio.jpg";
                    }
                    else
                    {
                        avatar = "default.png";
                    }
                Respuesta = new IdentityError { Code = "OK", Description = avatar };
                return JsonConvert.SerializeObject(Respuesta);

            }
                catch (Exception ex)
                {
                    Respuesta = new IdentityError { Code = "Error", Description = ex.ToString() };
                    return JsonConvert.SerializeObject(Respuesta);
                }
            
        }

        [HttpPost]
        public async Task<String> Agregar(Socio model)
        {
            Respuesta = new IdentityError();
            var consulta = _context.Socio.Where(x => x.Email.Equals(model.Email)).Count();
            //validaciones
            if (!model.Email.Contains('@') || !model.Email.Contains('.'))
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "* Agrega una direccion de correo valida";
                return JsonConvert.SerializeObject(Respuesta);
            }
            else if (consulta > 0)
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "Ya existe un usuario con ese correo registrado.";
                return JsonConvert.SerializeObject(Respuesta);
            }

            try
            {
                var path = Environment.WebRootPath + "\\Inscripciones\\";


                if (model.PictureAvatar != "default.png")//cambiar el nombre de la imagen
                {
                    var fullPath = path + model.FullName + ".jpg";
                    System.IO.File.Move(path + "NewSocio.jpg", fullPath);
                    model.PictureAvatar = model.FullName + ".jpg";

                }
                



                await _context.Socio.AddAsync(model);
                _context.SaveChanges();
                Respuesta.Code = "OK";
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
        public String Editar(Socio Socio)
        {
            Respuesta = new IdentityError();

            Socio SocioAntiguo = _context.Socio.Where(x => x.SocioID == Socio.SocioID).FirstOrDefault();

            //validaciones
            if (!Socio.Email.Contains('@') || !Socio.Email.Contains('.'))
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "* Agrega una direccion de correo valida";
                return JsonConvert.SerializeObject(Respuesta);
            }

            try
            {
                var path = Environment.WebRootPath + "\\Inscripciones\\";
                string fullPath;

                if (Socio.PictureAvatar != "default.png")// si subio imagen
                {

                    if (System.IO.File.Exists(path + SocioAntiguo.FullName + ".jpg"))//verificamos si existe la vieja imagen si es asi la borramos y renombramos la nueva subida
                    {
                        System.IO.File.Delete(path + SocioAntiguo.FullName + ".jpg");
                        fullPath = path + Socio.FullName + ".jpg";
                        System.IO.File.Move(path + "NewSocio.jpg", fullPath);
                        Socio.PictureAvatar = Socio.FullName + ".jpg";
                    }
                    else// si no existe una vieja imagen quiere decir que tiene la imagen default y solamente renombramos la nueva subida
                    {
                        fullPath = path + Socio.FullName + ".jpg";
                        System.IO.File.Move(path + "NewSocio.jpg", fullPath);
                        Socio.PictureAvatar = Socio.FullName + ".jpg";
                    }

                }
                else// si no subio imagen cambiamos la ruta de la vieja imagen
                {
                    if (System.IO.File.Exists(path + SocioAntiguo.FullName + ".jpg"))//si existe una vieja imagen le cambiamos el nombre porl el nuevo fullname
                    {
                        fullPath = path + Socio.FullName + ".jpg";
                        System.IO.File.Move(path + SocioAntiguo.PictureAvatar, fullPath);
                        Socio.PictureAvatar = Socio.FullName + ".jpg";
                    }
                    else//si no existe una vieja imagen  y como tambien no subio nueva le asiganmos la default
                    {
                        Socio.PictureAvatar = "default.png";
                    }
                }

                //update del socio
                SocioAntiguo.FullName = Socio.FullName;
                SocioAntiguo.Email = Socio.Email;
                SocioAntiguo.PhoneNumber = Socio.PhoneNumber;
                SocioAntiguo.BirthDate = Socio.BirthDate;
                SocioAntiguo.PictureAvatar = Socio.PictureAvatar;
                



                _context.Socio.Update(SocioAntiguo);
                _context.SaveChanges();
                Respuesta.Code = "OK";
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
        public async Task<String> Eliminar(int Id)
        {
            try
            {
                var Socio = _context.Socio.Where(x => x.SocioID.Equals(Id)).FirstOrDefault();
                _context.Socio.Remove(Socio);
                await _context.SaveChangesAsync();

                //eliminar foto
                var path = Environment.WebRootPath + "\\Inscripciones\\";
                System.IO.File.Delete(path + Socio.FullName + ".jpg");

                Respuesta = new IdentityError { Code = "OK" };
                return JsonConvert.SerializeObject(Respuesta);
            }
            catch (Exception ex)
            {
                Respuesta = new IdentityError { Code = "Error", Description= ex.Message};
                return JsonConvert.SerializeObject(Respuesta);

                throw;
            }
        }

        [HttpGet]
        public String GetSocio(int Id)
        {
            var Socios = _context.Socio.Where(x => x.SocioID == Id);
            var Json = JsonConvert.SerializeObject(Socios.FirstOrDefault());
            var Respuesta = Json.Replace("T00:00:00", null);
            
            return Respuesta;
        }

        [HttpPost]
        public List<string> Filtrar(int Pagina, int Registros, string Value)
        {
            Object[] Objects = new Object[3];
            var data = SearchSocios(Value);
            List<String> Retorno = new List<string>();

            if (data.Count > 0)
            {
                Objects = new GenericClass.Paginador<Socio>().paginar(data, Pagina, Registros, "Inscripciones");

                //sacar un solo string de los datos
                String FinalData = "";
                foreach (var item in (List<Socio>)Objects[2])
                {
                    FinalData += item.Card();
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

        private List<Socio> SearchSocios(string value)
        {

            if (value == null || value == "null")
                return _context.Socio.ToList();
            else
                return _context.Socio.Where(x => x.FullName.StartsWith(value)).ToList();


        }
    }
}
