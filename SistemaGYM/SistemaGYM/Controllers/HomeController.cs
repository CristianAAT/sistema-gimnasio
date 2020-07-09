using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SistemaGYM.Data;
using SistemaGYM.Models;

namespace SistemaGYM.Controllers
{
    public class HomeController : Controller
    {
        IServiceProvider ServiceProvider;
        private ApplicationDbContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            ServiceProvider = serviceProvider;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            CreateRoles(ServiceProvider).Wait();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding customs roles : Question 1
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Gerente", "Miembro" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 2
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

        }

        [HttpGet]
        public List<string> Productos()
        {
            Object[] Objects = new Object[3];
            var data = _context.Producto.ToList();
            List<String> Retorno = new List<string>();

            if (data.Count > 0)
            {
                Objects = new GenericClass.Paginador<Producto>().paginar(data, 1, 9, "Productos");

                //sacar un solo string de los datos
                String FinalData = "";
                foreach (var item in (List<Producto>)Objects[2])
                {
                    FinalData += item.Presentacion();
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
    }
}
