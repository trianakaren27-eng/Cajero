using System;

namespace Cajero

{
    /// <summary>
    /// Clase principal para iniciar el cajero-interactua con el usuario
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            //Cajero es:creacion del objeto de la clase cajero para llamar los metodos

            //Cajero cajero = new Cajero();
            int opcion = 0;

            //do:haga algomientras se cumpla la condicion del while--minimetodo bucle

            do
            {
                Console.WriteLine("---CAJERO AUTOMATICO DANI---");
                Console.WriteLine("1. Crear usuario");
                Console.WriteLine("2. Iniciar sesion");
                Console.WriteLine("3. Salir");
                Console.Write("Selecciona una opcion");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.WriteLine("\n==============================");
                    Console.WriteLine("Opción inválida, intente de nuevo.");
                    Console.WriteLine("==============================\n");

                    continue;
                }
                //dependiendo el numero que escoja por cual se va
                switch (opcion)
                {
                    case 1:
                        Console.Write("Ingrese documento: ");
                        string docNuevo = Console.ReadLine();
                        Console.Write("Ingrese clave: ");
                        string claveNueva = LeerClaveOculta();
                        cajero.CrearUsuario(docNuevo, claveNueva);
                        break;

                    case 2:
                        Console.Write("Documento: ");
                        string doc = Console.ReadLine();
                        Console.Write("Clave: ");
                        string clave = LeerClaveOculta();

                        if (cajero.IniciarSesion(doc, clave))
                        {
                            MostrarMenuCajero(cajero);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n┌───────────────────────────────┐");
                            Console.WriteLine("│  Inicio de sesión fallido.    │");
                            Console.WriteLine("└───────────────────────────────┘\n");
                            Console.ResetColor();

                        }
                        break;

                    case 3:
                        Console.WriteLine("👋 Gracias por usar el cajero. Hasta pronto.");
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n┌───────────────────────────┐");
                        Console.WriteLine("│   Opción no válida.       │");
                        Console.WriteLine("└───────────────────────────┘\n");
                        Console.ResetColor();

                        break;
                }

                Console.WriteLine();


            } while (opcion != 3);
        }
    }
}
