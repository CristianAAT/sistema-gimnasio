using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGYM.Data;
using SistemaGYM.Models;

namespace SistemaGYM.Controllers
{
    [Authorize(Roles = "Admin, Gerente")]
    public class EntradasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Entrada model;
        private IdentityError Respuesta;

        public EntradasController(ApplicationDbContext context){ _context = context; }

        public IActionResult Entradas()//Index View
        {
            model = new Entrada();
            return View(model);
        }

        [HttpPost]
        public async Task<String> Agregar(Entrada Data)
        {
            Respuesta = new IdentityError();
            try
            {
                await _context.Entrada.AddAsync(Data);

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
                var Entrada = _context.Entrada.Where(x => x.EntradaID.Equals(Id)).FirstOrDefault();

                _context.Entrada.Remove(Entrada);
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
        public List<string> Filtrar(int Pagina, int Registros, string Value)
        {
            Object[] Objects = new Object[3];
            var data = SearchEntradas(Value);
            List<String> Retorno = new List<string>();

            if (data.Count > 0)
            {
                Objects = new GenericClass.Paginador<Entrada>().paginar(data, Pagina, Registros, "Entradas");

                //sacar un solo string de los datos
                String FinalData = "";
                foreach (var item in (List<Entrada>)Objects[2])
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

        private List<Entrada> SearchEntradas(string value)
        {


            if (value == null || value == "null")
                return _context.Entrada.ToList();
            else
            {
                DateTime Date = DateTime.Parse(value);
                return _context.Entrada.Where(x => x.DayDate.Equals(Date)).ToList();
            }
        }
    }
}