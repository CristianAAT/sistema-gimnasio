using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGYM.Data;
using SistemaGYM.Models;

namespace SistemaGYM.Controllers
{
    [Authorize(Roles = "Admin, Gerente")]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Producto model;
        private IdentityError Respuesta;
        private IWebHostEnvironment Environment;

        public ProductosController(ApplicationDbContext context, IWebHostEnvironment Environment)
        {
            _context = context;
            this.Environment = Environment;
        }

        public IActionResult Productos()
        {
            model = new Producto();
            return View(model);
        }

        [HttpPost]
        public String Imagen(IFormFile file)
        {
            string avatar = "";
            try
            {
                //insertar foto
                var path = Environment.WebRootPath + "\\Productos\\";
                if (file != null)
                {
                    path = path + "NewProducto" + ".jpg";

                    using (var stream = System.IO.File.Create(path))
                    {
                        file.CopyTo(stream);
                        stream.Flush();
                    }
                    avatar = "NewProducto.jpg";
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
        public async Task<String> Agregar(Producto model)
        {
            Respuesta = new IdentityError();
            var consulta = _context.Producto.Where(x => x.Name.Equals(model.Name)).Count();
            //validaciones
            if (consulta > 0)
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "Ya existe un producto con ese nombre registrado.";
                return JsonConvert.SerializeObject(Respuesta);
            }

            try
            {
                var path = Environment.WebRootPath + "\\Productos\\";


                if (model.ImageStr != "default.png")//cambiar el nombre de la imagen
                {
                    var fullPath = path + model.Name + ".jpg";
                    System.IO.File.Move(path + "NewProducto.jpg", fullPath);
                    model.ImageStr = model.Name + ".jpg";

                }




                await _context.Producto.AddAsync(model);
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
        public String Editar(Producto Producto)
        {
            Respuesta = new IdentityError();

            Producto ProductoAntiguo = _context.Producto.Where(x => x.ProductoID == Producto.ProductoID).FirstOrDefault();

            //validaciones

            try
            {
                var path = Environment.WebRootPath + "\\Productos\\";
                string fullPath;

                if (Producto.ImageStr != "default.png" && Producto.ImageStr != null)// si subio imagen
                {

                    if (System.IO.File.Exists(path + ProductoAntiguo.Name + ".jpg"))//verificamos si existe la vieja imagen si es asi la borramos y renombramos la nueva subida
                    {
                        System.IO.File.Delete(path + ProductoAntiguo.Name + ".jpg");
                        fullPath = path + Producto.Name + ".jpg";
                        System.IO.File.Move(path + "NewProducto.jpg", fullPath);
                        Producto.ImageStr = Producto.Name + ".jpg";
                    }
                    else// si no existe una vieja imagen quiere decir que tiene la imagen default y solamente renombramos la nueva subida
                    {
                        fullPath = path + Producto.Name + ".jpg";
                        System.IO.File.Move(path + "NewProducto.jpg", fullPath);
                        Producto.ImageStr = Producto.Name + ".jpg";
                    }

                }
                else// si no subio imagen cambiamos la ruta de la vieja imagen
                {
                    if (System.IO.File.Exists(path + ProductoAntiguo.Name + ".jpg"))//si existe una vieja imagen le cambiamos el nombre porl el nuevo fullname
                    {
                        fullPath = path + Producto.Name + ".jpg";
                        System.IO.File.Move(path + ProductoAntiguo.ImageStr, fullPath);
                        Producto.ImageStr = Producto.Name + ".jpg";
                    }
                    else//si no existe una vieja imagen  y como tambien no subio nueva le asiganmos la default
                    {
                        Producto.ImageStr = "default.png";
                    }
                }

                //update del socio
                ProductoAntiguo.Name = Producto.Name;
                ProductoAntiguo.Cost = Producto.Cost;
                ProductoAntiguo.Stock = Producto.Stock;
                ProductoAntiguo.ImageStr = Producto.ImageStr;




                _context.Producto.Update(ProductoAntiguo);
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
                var Producto = _context.Producto.Where(x => x.ProductoID.Equals(Id)).FirstOrDefault();
                _context.Producto.Remove(Producto);
                await _context.SaveChangesAsync();

                //eliminar foto
                var path = Environment.WebRootPath + "\\Productos\\";
                System.IO.File.Delete(path + Producto.ProductoID + ".jpg");


                Respuesta = new IdentityError { Code = "OK" };
                return JsonConvert.SerializeObject(Respuesta);
            }
            catch (Exception ex)
            {
                Respuesta = new IdentityError { Code = "Error", Description = ex.Message };
                return JsonConvert.SerializeObject(Respuesta);

                throw;
            }
        }

        [HttpGet]
        public String GetProducto(int Id)
        {
            var Producto = _context.Producto.Where(x => x.ProductoID == Id);
            return JsonConvert.SerializeObject(Producto.FirstOrDefault());
        }

        [HttpPost]
        public List<string> Filtrar(int Pagina, int Registros, string Value)
        {
            Object[] Objects = new Object[3];
            var data = SearchProductos(Value);
            List<String> Retorno = new List<string>();

            if (data.Count > 0)
            {
                Objects = new GenericClass.Paginador<Producto>().paginar(data, Pagina, Registros, "Productos");

                //sacar un solo string de los datos
                String FinalData = "";
                foreach (var item in (List<Producto>)Objects[2])
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

        private List<Producto> SearchProductos(string value)
        {

            if (value == null || value == "null")
                return _context.Producto.ToList();
            else
                return _context.Producto.Where(x => x.Name.StartsWith(value)).ToList();


        }
    }
}