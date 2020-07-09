using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGYM.Models
{
    public class Producto
    {
        public Producto()
        {

        }

        //attributes and mathods
        [Key]
        public int ProductoID { get; set; }
        [Required]
        [Display(Name = "Nombre del producto")]
        public String Name { get; set; }
        [Required]
        [Display(Name = "Costo del producto")]
        public float Cost { get; set; }
        [Required]
        [Display(Name = "Stock del producto")]
        public int? Stock { get; set; }

        public string ImageStr { get; set; }

        public string Card()
        {
            return "<div class='col-md-3 col-sm-6 col-xs-8 space-top-sm'>" +
                        "<div class='card card-body'>" +
                            "<img class='card-img-top img-fluid' src='" + ImageStr + "' data-holder-rendered='true' style='height: 180px; width: 100%; display: block;' />" +
                            "<div class'card-body'>" +
                                "<h5 class'card-title'>" + Name + "</h5>" +
                                "<p>Precio: " + Cost + "</p>" +
                                "<p>Cantidad: " + Stock + "</p>" +
                                "<a class='btn btn-outline-info' data-target='#mEditar' data-toggle='modal' onclick='Productos.GetProducto(\"" + ProductoID + "\")'>Editar</a>" +
                                "<a class='btn btn-outline-danger' data-target='#mEliminar' data-toggle='modal' onclick='Productos.SaveData(\"" + ProductoID + "\")'>Eliminar</a>" +
                            "</div>" +
                        "</div>" +
                    "</div>";
        }
        public string Presentacion()
        {
            return "<div class='col-md-3 col-sm-6 col-xs-8 space-top-sm'>" +
                        "<div class='card card-body'>" +
                            "<img class='card-img-top img-fluid' src='Productos/" + ImageStr + "' data-holder-rendered='true' style='height: 180px; width: 100%; display: block;' />" +
                            "<div class'card-body'>" +
                                "<h5 class'card-title'>" + Name + "</h5>" +
                                "<p>Precio: " + Cost + "</p>" +
                                "<p>Cantidad: " + Stock + "</p>" +
                            "</div>" +
                        "</div>" +
                    "</div>";
        }



        //Pediente: card para venta de producto.

    }
}
