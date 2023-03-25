
namespace Core.Utils
{
    using Core.Models.ClassApi;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Menu
    {
        #region MENU
        public static string MenuSolicitarDespacho()
        {
            Console.WriteLine("                   ********************************************");
            Console.WriteLine("                   |     BIENVENIDO A DATA TRANSFER           |");
            Console.WriteLine("                   | Por favor ingrese los datos solicitado:  |");
            Console.WriteLine("                   |                                          |");
            Console.WriteLine("                   ********************************************");
            Console.WriteLine("\n");

            string nombreDespacho = string.Empty;

            Console.WriteLine("Ingrese nombre del despacho:");
            nombreDespacho = Console.ReadLine();

            return nombreDespacho;
        }

        public static string MenuSeleccionarSerie(List<Series> series)
        {
            string serie = null;

            Console.WriteLine("                   **********************************************");
            Console.WriteLine("                   | Por favor seleccione la serie o sub serie: |");
            foreach (var s in series)
                Console.WriteLine("               |{0}. {1}                                    |", s.Codigo, s.Serie_SubSerie);
            Console.WriteLine("                   |                                            |");
            Console.WriteLine("                   **********************************************");
            Console.WriteLine("\n");
            serie = Console.ReadLine();

            return serie;
        }

        public static string MenuExpediente()
        {

            Console.WriteLine("                   **********************************************");
            Console.WriteLine("                   | Por favor digite el expediente:            |");
            Console.WriteLine("                   |                                            |");
            Console.WriteLine("                   **********************************************");
            Console.WriteLine("\n");
            string expediente = Console.ReadLine();
            return expediente;
        }

        public static string MenuIdArea()
        {

            Console.WriteLine("                   **********************************************");
            Console.WriteLine("                   | Por favor digite el id Area:               |");
            Console.WriteLine("                   |                                            |");
            Console.WriteLine("                   **********************************************");
            Console.WriteLine("\n");
            string idArea = Console.ReadLine();
            return idArea;

        }
        #endregion
    }
}