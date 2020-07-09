using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGYM.Data;

namespace SistemaGYM.Controllers
{
    public class ReportesController : Controller
    {
        private ApplicationDbContext _context;
        private IdentityError Respuesta;

        public ReportesController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult Reportes()
        {
            return View();
        }

        [HttpGet]
        public string getDataMonths()
        {
            DataTable Table = new DataTable();
            Table.Columns.Add("Mes", typeof(String));
            Table.Columns.Add("Mensualidad", typeof(Double));
            Table.Columns.Add("Inscripcion", typeof(Double));
            Table.Columns.Add("Visitas", typeof(Double));

            List<DateTime> Months = new List<DateTime>();
            for (int i = 11; i >= 0; i--)
            {
                Months.Add(DateTime.Now.AddMonths(-i));
            }

            List<Double> Mensualidades = new List<double>();
            foreach (var item in Months)
            {
 
                var Result = from x in _context.Membresia
                          where x.StartDate.Month.Equals(item.Month) && x.StartDate.Year.Equals(item.Year)
                          select x;

                Double Total = 0;
                foreach (var Mem in Result)
                {
                    Total += Mem.Cost;
                }
                Mensualidades.Add(Total);
            }

            List<Double> Inscripciones = new List<double>();
            foreach (var item in Months)
            {
                var Result = from x in _context.Socio
                             where x.InscriptionDate.Month.Equals(item.Month) && x.InscriptionDate.Year.Equals(item.Year)
                             select x;

                Double Total = 0;
                foreach (var Ins in Result)
                {
                    Total += Ins.InscriptionCost;
                }
                Inscripciones.Add(Total);
            }

            List<Double> Visitas = new List<double>();
            foreach (var item in Months)
            {
                var Result = from x in _context.Entrada
                             where x.DayDate.Month.Equals(item.Month) && x.DayDate.Year.Equals(item.Year) && x.VisitCost > 0
                             select x;

                Double Total = 0;
                foreach (var entrada in Result)
                {
                    Total += entrada.VisitCost;
                }
                Visitas.Add(Total);
            }

            for (int i = 0; i < 12; i++)
            {
                Table.Rows.Add(Months.ElementAt(i).ToString("yyyy MMMM"), Mensualidades.ElementAt(i), Inscripciones.ElementAt(i), Visitas.ElementAt(i));
            }

            return JsonConvert.SerializeObject(Table);
        }

        [HttpPost]
        public String GetReportes(string Mes, string Anio)
        {
            List<Reporte> Reportes = new List<Reporte>();
            Double Total = 0;
            string Table = "";

            //Sacar Mensualidades vendidas en el mes
            var Mensualidades = _context.Membresia.Where(x => x.StartDate.Month.Equals(Convert.ToInt32(Mes)) && x.StartDate.Year.Equals(Convert.ToInt32(Anio))).ToList();
            foreach (var item in Mensualidades)
            {
                Reportes.Add(new Reporte
                {
                    Date = item.StartDate,
                    Name = item.SocioName(),
                    Product = "Mensualidad",
                    Cost = item.Cost
                });
                Total += item.Cost;
            }

            //Sacar inscripcciones
            var Inscripciones = _context.Socio.Where(x => x.InscriptionDate.Month.Equals(Convert.ToInt32(Mes)) && x.InscriptionDate.Year.Equals(Convert.ToInt32(Anio))).ToList();
            foreach (var item in Inscripciones)
            {
                Reportes.Add(new Reporte
                {
                    Date = item.InscriptionDate,
                    Name = item.FullName,
                    Product = "Inscripción",
                    Cost = item.InscriptionCost
                });
                Total += item.InscriptionCost;
            }

            //sacar ventas de visitas al gym
            var Visitas = _context.Entrada.Where(x => 
                x.DayDate.Month.Equals(Convert.ToInt32(Mes)) &&
                x.DayDate.Year.Equals(Convert.ToInt32(Anio)) &&
                x.VisitCost > 0
                ).ToList();
            foreach (var item in Visitas)
            {
                Reportes.Add(new Reporte 
                {
                    Date = item.DayDate,
                    Name = "Visitante anonimo",
                    Product = "Visita al gimnasio",
                    Cost = item.VisitCost
                });
            }

            if (Mensualidades.Count == 0 && Inscripciones.Count == 0 && Visitas.Count == 0)
            {
                Table = "<tr class='table-dark'><td colspan='4'><strong><h4><center>No existen ventas para esta fecha. =(</center></h4></strong></td></tr>";
                return Table;
            }
            else
            {
            Reportes = (from R in Reportes orderby R.Date select R).ToList();
            foreach (var item in Reportes)
            {
                Table += item.Row();
            }

                Table += "<tr class='table-dark'>" +
                               "<td colspan='4'><strong><h5><center>INGRESOS TOTALES:   " + Total +"</center></h5><strong></td>" +
                         "</tr>";
                return Table;
            }
        }

        public class Reporte
        {
            public DateTime Date { get; set; }
            public String Name { get; set; }
            public String Product { get; set; }
            public Double Cost { get; set; }

            public string Row()
            {
                return "<tr class='table-active'>" +
                           "<td>" + Date.ToLongDateString() + "</td>" +
                           "<td>" + Name + "</td>" +
                           "<td>" + Product + "</td>" +
                           "<td>" + Cost + "</td>" +
                       "</tr>";
            }
        }

    }
}