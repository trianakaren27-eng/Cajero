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
            //Cajero es el nombre de la clase y cajero es el nombre del objeto 

            CajeroLogica cajero = new CajeroLogica();
            int opcion = 0;

            //do:haga algoritmo mientras se cumpla la condicion del while--minimetodo bucle

            do
            {
                Console.WriteLine("---CAJERO AUTOMATICO DANI---");
                Console.WriteLine("1. Crear usuario");
                Console.WriteLine("2. Iniciar sesion");
                Console.WriteLine("3. Salir");
                Console.Write("Selecciona una opcion : ");


                // si el digito se puede convertir a entero lo asigna a opcion, de lo contrario falla y reinicia el bucle
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

                        // el if lo que hace es devolver verdadero o falso para ver si entra en la logica del menu del cajero 
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
                        Console.WriteLine(" Gracias por usar el cajero. Hasta pronto.");
                        break;
                        // opcion por defecto cuando no coincide la opciopn del usuario con ningun case 
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n┌───────────────────────────┐");
                        Console.WriteLine("│   Opción no válida.       │");
                        Console.WriteLine("└───────────────────────────┘\n");
                        Console.ResetColor();

                        break;
                }

                Console.WriteLine();

                // mientras la opcion sea diferente de tres va a repetir el bucle do 

            } while (opcion != 3);
        }



        public static string LeerClaveOculta()
        {
            string clave = "";
            ConsoleKeyInfo tecla;

            do
            {
                tecla = Console.ReadKey(true); // cuando se encuentra en true, no muestra lo que el usuario digita

                if (tecla.Key == ConsoleKey.Enter) // tan pronto el usuario presione enter, el sistema asimila que ya acabo de escribir
                    break;

                if (tecla.Key == ConsoleKey.Backspace && clave.Length > 0)
                {
                    clave = clave.Substring(0, clave.Length - 1); // en esta seccion, cuando el usuario le da borrar, borra la ultima posicion en memoria asi como el ultimo asterizco de la clave
                    Console.Write("\b \b");
                }
                else if (!char.IsControl(tecla.KeyChar)) // agrega un asterizco a la clave, reemplazando la letra por un * 
                {
                    clave += tecla.KeyChar;
                    Console.Write("*");
                }

            } while (true); // se encarga de mantener el bucle hasta que se presione enter

            Console.WriteLine();
            return clave;
        }

        /// <summary>
        /// Submenú de operaciones del cajero --> metodo de clase porque se encuentra en la misma clase donde se llama !!!!!
        /// </summary>
        static void MostrarMenuCajero(CajeroLogica cajero)
        {
            int opc;
            do
            {
                Console.WriteLine("\n--- Menú Cajero ---");
                Console.WriteLine("1. Depósito");
                Console.WriteLine("2. Retiro");
                Console.WriteLine("3. Consultar Saldo");
                Console.WriteLine("4. Últimos 5 Movimientos");
                Console.WriteLine("5. Cambiar Clave");
                Console.WriteLine("6. Cerrar Sesión");
                Console.Write("Seleccione opción: ");

                if (!int.TryParse(Console.ReadLine(), out opc))
                {
                    Console.WriteLine(" Opción inválida.");
                    continue;
                }

                switch (opc)
                {
                    case 1:
                        Console.Write("Monto a depositar: ");
                        decimal dep = decimal.Parse(Console.ReadLine());
                        cajero.Depositar(dep);
                        break;
                    case 2:
                        Console.Write("Monto a retirar: ");
                        decimal ret = decimal.Parse(Console.ReadLine());
                        cajero.Retirar(ret);
                        break;
                    case 3:
                        cajero.ConsultarSaldo();
                        break;
                    case 4:
                        cajero.ConsultarMovimientos();
                        break;
                    case 5:
                        Console.Write("Nueva clave: ");
                        string nuevaClave = LeerClaveOculta();
                        // cuando dice cajero.metodo es porque el metodo viene de la clase de el tipo de objeto que es CLASE nombre_objeto = NEW CLASE();
                        cajero.CambiarClave(nuevaClave);
                        break;
                    case 6:
                        Console.WriteLine("🔒 Sesión cerrada.");
                        break;
                    default:
                        Console.WriteLine("❌ Opción no válida.");
                        break;
                }
            } while (opc != 6);
        }

    }
}
