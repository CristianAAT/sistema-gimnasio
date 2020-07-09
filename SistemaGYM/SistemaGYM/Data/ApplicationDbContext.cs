using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaGYM.Models;

namespace SistemaGYM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Socio> Socio { get; set; }
        public DbSet<Membresia> Membresia { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Entrada> Entrada { get; set; }
    }
}
