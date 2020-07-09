using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class MembresiasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Membresia model;
        private IdentityError Respuesta;

        public MembresiasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Membresias()//Metodo de acceso al controlador.
        {
            model = new Membresia(_context);
            return View(model);
        }

        [HttpPost]
        public async Task<String> Agregar(Membresia Data)
        {


            Respuesta = new IdentityError();

            //Validar si socio es diferente a null
            if (Data.SocioID == 0)
            {
                Respuesta.Code = "ERROR";
                Respuesta.Description = "Elige un socio a quien asignarle la membresia/mensualidad.";
                return JsonConvert.SerializeObject(Respuesta);
            }

            try
            {
                Socio SocioDB = _context.Socio.Where(x => x.SocioID.Equals(Data.SocioID)).ToList().FirstOrDefault();
                await _context.Membresia.AddAsync(Data);
                SocioDB.State = true;
                _context.Socio.Update(SocioDB);

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

        [HttpDelete]
        public async Task<String> Eliminar(int Id)
        {
            try
            {
                var Membresia = _context.Membresia.Where(x => x.MembresiaID.Equals(Id)).FirstOrDefault();
                //cambiar estado del socio
                var Socio = _context.Socio.Where(x => x.SocioID.Equals(Membresia.SocioID)).FirstOrDefault();
                Socio.State = false;

                _context.Socio.Update(Socio);
                await _context.SaveChangesAsync();
                _context.Membresia.Remove(Membresia);
                await _context.SaveChangesAsync();

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

        [HttpPost]
        public async Task<List<SelectListItem>> BuscarSocios(string Value)
        {
            List<string> Retorno = new List<string>();
            List<Socio> Socios = await _context.Socio.Where(x => x.FullName.StartsWith(Value)).ToListAsync();
            List<SelectListItem> asd = new List<SelectListItem>();
            foreach (var item in Socios)
            {
                asd.Add(new SelectListItem { Value = item.SocioID.ToString(), Text = item.FullName });
                Retorno.Add(item.FullName);
            }

            return asd;
        }

        [HttpPost]
        public List<string> Filtrar(int Pagina, int Registros, string Value)
        {
            Object[] Objects = new Object[3];
            var data = SearchMembresias(Value);
            List<String> Retorno = new List<string>();

            if (data.Count > 0)
            {
                Objects = new GenericClass.Paginador<Membresia>().paginar(data, Pagina, Registros, "Membresias");

                //sacar un solo string de los datos
                String FinalData = "";
                foreach (var item in (List<Membresia>)Objects[2])
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

        private List<Membresia> SearchMembresias(string value)
        {


            if (value == null || value == "null")
                return _context.Membresia.ToList();
            else
                return (from a in _context.Membresia
                        join b in _context.Socio on a.SocioID equals b.SocioID
                        where b.FullName.Contains(value)
                        select a).Distinct().ToList();

        }


    }
}
