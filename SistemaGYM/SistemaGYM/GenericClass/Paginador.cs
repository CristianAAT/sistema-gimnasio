using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaGYM.GenericClass
{
    public class Paginador<T>
    {
        private int Registros_por_pag = 3;
        private int Enlaces_por_pag = 5;
        private int Pag_actual;
        private string Nav_anterior = " &laquo; Anterior ";
        private string Nav_siguiente = " Siguiente &raquo; ";
        private string Nav_primera = " &laquo; Primero ";
        private string Nav_ultima = " Ultimo &raquo; ";
        private string Navegacion = null;

        public Object[] paginar(List<T> table, int Pagina, int Registros, string ModelClass/*Model class es para buscar la clase de javascript que tendran los vinculos*/)
        {
            if (Registros > 0)
                Registros_por_pag = Registros;

            if (Pagina.Equals(0))
                Pag_actual = 1;
            else
                Pag_actual = Pagina;

            int Pag_total_reg = table.Count;
            int Pag_total_regs = Pag_total_reg;

            if ((Pag_total_reg % Registros_por_pag) > 0)
                Pag_total_regs += 2;

            int Pag_total_pags = Pag_total_regs / Registros_por_pag;


            if (Pag_actual != 1)
            {
                int Pag_url = 1;
                Navegacion += "<a class='btn btn-info btn-sm' onclick='" + ModelClass + ".Filtrar(" + Pag_url + ',' + " " + ")'>" + Nav_primera + "</a> ";
                Pag_url = Pag_actual - 1;
                Navegacion += "<a class='btn btn-info btn-sm' onclick='" + ModelClass + ".Filtrar(" + Pag_url + ','  + " " + ")'>" + Nav_anterior + "</a>  ";
            }

            double Value = (Enlaces_por_pag / 2);
            int Pag_nav_intervalo = Convert.ToInt16(Math.Round(Value));
            int Pag_nav_desde = Pag_actual - Pag_nav_intervalo;
            int Pag_nav_hasta = Pag_actual + Pag_nav_intervalo;

            if (Pag_nav_desde < 1)
            {
                Pag_nav_hasta -= (Pag_nav_desde - 1);
                Pag_nav_desde = 1;
            }

            if (Pag_nav_hasta > Pag_total_pags)
            {
                Pag_nav_desde -= (Pag_nav_hasta - Pag_total_pags);
                Pag_nav_hasta = Pag_total_pags;

                if (Pag_nav_desde < 1)
                    Pag_nav_desde = 1;

            }

            for (int Pag_i = Pag_nav_desde; Pag_i <= Pag_nav_hasta; Pag_i++)
            {
                if (Pag_i == Pag_actual)
                    Navegacion += "<span class='btn btn-success' disabled='disabled'>" + Pag_i + "</span>";
                else
                    Navegacion += "<a class='btn btn-secondary' onclick='" + ModelClass + ".Filtrar(" + Pag_i  + ',' + " " + ")'>" + Pag_i + "</a>";
            }

            if (Pag_actual < Pag_total_pags)
            {
                int Pag_url = Pag_actual + 1;
                Navegacion += "  <a class='btn btn-info btn-sm' onclick='" + ModelClass + ".Filtrar(" + Pag_url + ',' + " " + ")'>" + Nav_siguiente + "</a>";

                Pag_url = Pag_total_pags;
                Navegacion += " <a class='btn btn-info btn-sm' onclick='" + ModelClass + ".Filtrar(" + Pag_url + ',' + " " + ")'>" + Nav_ultima + "</a>";
            }

            int Pag_inicial = (Pag_actual - 1) * Registros_por_pag;

            var query = table.Skip(Pag_inicial).Take(Registros_por_pag).ToList();


            
            String Pag_info = "Pagina <b>" + Pag_actual + "</b> de <b>" + Pag_total_pags + "</b> paginas." +
                              "<b> " + Pag_total_reg+ "</b> Registros totales <br />";

            Object[] data = { Pag_info, Navegacion, query };
            return data;
        }
    }
}
