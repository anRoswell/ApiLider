namespace Core.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class  Consola
    {
        public static void ShowEquivalenciaIntegrador(string nombreDespacho, string serieSeleccionada, string IdArea, string IdCiudad, string IdUsuario, string IdTrd, string IdVersionTrd)
        {
            
            Console.SetCursorPosition(0, 0);
            Console.WriteLine($"*************************************************************** \r");
            Console.WriteLine($"Equivalencia Integrador \r");
            Console.WriteLine($"Despacho       : {nombreDespacho} \r");
            Console.WriteLine($"Serie/Subserie : {serieSeleccionada} \r");
            Console.WriteLine($"IdArea         : {IdArea}       - IdCiudad   : {IdCiudad} \r");
            Console.WriteLine($"IdUsuario      : {IdUsuario}    - IdTrd      : {IdTrd} \r");
            Console.WriteLine($"IdVersionTrd   : {IdVersionTrd} \r");
            Console.WriteLine($"*************************************************************** \r");
        }

        public static void ShowMostrarExpediente(string expediente)
        {
            Console.SetCursorPosition(0, 10);            
            Console.WriteLine($"Expediente       : {expediente} \r");                        
        }

        public static void ShowExpediente(int contadorDespachos, int totalDespachos)
        {
            Console.SetCursorPosition(0, 11);
            Console.WriteLine($"Procesando {contadorDespachos} de {totalDespachos} expedientes \r");
        }


        public static void ShowSeriSubSerie(int contadorSeriSubSerie, int totalSeriSubSerie)
        {
            Console.SetCursorPosition(0, 12);
            Console.WriteLine($"Procesando {contadorSeriSubSerie} de {totalSeriSubSerie} SerieSubSerieExpediente \r");
        }

        public static void ShowFolders(int contador, int total)
        {
            Console.SetCursorPosition(0, 13);
            Console.WriteLine($"Procesando {contador} de {total} folders \r");
        }

        public static void ShowNoteBooks(int contador, int total)
        {
            Console.SetCursorPosition(0, 14);
            Console.WriteLine($"Procesando {contador} de {total} NoteBook \r");
        }

        public static void ShowNoteBookList(int contador, int? total)
        {
            Console.SetCursorPosition(0, 15);
            Console.WriteLine($"Procesando {contador} de {total} NoteBookList \r");
        }

        public static void ShowFiles(int contador, int total)
        {
            Console.SetCursorPosition(0, 16);
            Console.WriteLine($"Procesando {contador} de {total} Files \r");
        }
    }
}
