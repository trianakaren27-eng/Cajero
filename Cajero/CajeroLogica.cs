using System;
using System.Collections.Generic;
using System.IO;


namespace Cajero
{
    /// <summary>
    /// Clase CajeroLogica: Contiene la lógica principal del cajero automático
    /// </summary>
    /// 
    public class CajeroLogica
    {

        private Usuario usuarioActual;
        private const string ArchivoUsuarios = "usuarios.txt";
        private const string CarpetaMovimientos = "movimientos";


        /// <summary>
        /// Condicionales encargados de verificar si existe la carpeta y el archivo, si no existen los crea
        /// </summary>
        public CajeroLogica()
        {
            if (!Directory.Exists(CarpetaMovimientos))
                Directory.CreateDirectory(CarpetaMovimientos);

            if (!File.Exists(ArchivoUsuarios))
                File.Create(ArchivoUsuarios).Close();
        }

        /// <summary>
        /// Funcion encargada de crear un nuevo usuario en sistema 
        /// </summary>
        public void CrearUsuario(string documento, string clave)
        {
            using (StreamWriter sw = File.AppendText(ArchivoUsuarios))
            {
                // aqui se añade en el archivo txt el documento;clave;0 el cero hace referencia al saldo inicial de ese usuario 
                sw.WriteLine($"{documento};{clave};0");
            }
            Console.WriteLine("\n==============================");
            Console.WriteLine(" Usuario creado correctamente.");
            Console.WriteLine("==============================\n");

        }

        /// <summary>
        /// Funcion encargada de iniciar sesion de un usuario registrado en el sistema 
        /// </summary>
        public bool IniciarSesion(string documento, string clave)
        {
            //bucle que se encarga de recorrer cada una de las lineas del archivo txt hasta que encuentre una que coincida con los datos dados por el usuario 
            foreach (var linea in File.ReadAllLines(ArchivoUsuarios))
            {
                var datos = linea.Split(';');
                if (datos[0] == documento && datos[1] == clave)
                {
                    // creacion de nuevo objeto tipo usuario apartir de los datos encontrados en una de las lineas del archivo txt
                    usuarioActual = new Usuario(datos[0], datos[1], decimal.Parse(datos[2]));
                    CargarMovimientos();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Funcion encargada de depositar dinero en la cuenta del usuario 
        /// </summary>
        public void Depositar(decimal monto)
        {
            usuarioActual.Saldo += monto;
            usuarioActual.RegistrarMovimiento($"Depósito de {monto:C}");
            GuardarCambios();
            GuardarMovimiento($"\n-------------------------\n Deposito realizado de: {monto:C}\n-------------------------\n");

        }

        /// <summary>
        /// Funcion encargada de retirar dinero de la cuenta del usuario 
        /// </summary>
        public void Retirar(decimal monto)
        {
            // valida si el saldo que hay actualmente en el txt es mayor o igual a lo que se pide o se quiere retirar
            if (usuarioActual.Saldo >= monto)
            {
                usuarioActual.Saldo -= monto;
                usuarioActual.RegistrarMovimiento($"Retiro de {monto:C}");
                GuardarCambios();
                GuardarMovimiento($"\n====== RETIRO ======\n Monto: {monto:C}\n====================\n");

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n┌─────────────────────────────┐");
                Console.WriteLine("│  Fondos insuficientes.     │");
                Console.WriteLine("└─────────────────────────────┘\n");
                Console.ResetColor();

            }
        }

        /// <summary>
        /// Funcion encargada de consultar el saldo del usuario
        /// </summary>
        public void ConsultarSaldo()
        {
            Console.WriteLine("\n=============================");
            Console.WriteLine($" Su saldo actual es: {usuarioActual.Saldo:C}");
            Console.WriteLine("=============================\n");

        }

        /// <summary>
        /// Funcion encargada de consultar los ultimos 5 movimientos del usuario
        /// </summary>
        public void ConsultarMovimientos()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n Últimos 5 movimientos:\n");
            Console.ResetColor();

            foreach (var mov in usuarioActual.Movimientos)
            {
                Console.WriteLine($"  {mov}");
            }
        }


        /// <summary>
        /// Funcion encargada de cambiar la contraseña del usuario logueado 
        /// </summary>
        public void CambiarClave(string nuevaClave)
        {
            usuarioActual.Clave = nuevaClave;
            usuarioActual.RegistrarMovimiento("Cambio de clave");
            GuardarCambios();
            GuardarMovimiento("Cambio de clave");
        }


        /// <summary>
        /// Guarda los datos actualizados del usuario en el archivo de usuarios
        /// </summary>
        private void GuardarCambios()
        {

            // se crea la lista lineas que va a guardar todos los registros del txt
            var lineas = new List<string>();
            // se crea el bucle que va a recorrer linea a linea el txt y va a ir guardando en memoria cada fila del txt
            foreach (var linea in File.ReadAllLines(ArchivoUsuarios))
            {
                var datos = linea.Split(';');
                // esta condicion verifica que cedula coincide con la cedula del usuario logueado mediante el objeto de tipo usuario llamado usuarioActual
                if (datos[0] == usuarioActual.NumeroDocumento)
                {
                    // aqui se hace la modificacion de la fila que coincida modificando lo que se haya cambiado en el objeto
                    lineas.Add($"{usuarioActual.NumeroDocumento};{usuarioActual.Clave};{usuarioActual.Saldo}");
                }
                else
                {
                    lineas.Add(linea);
                }
            }
            // aqui ya se tiene la nueva lista en memoria llamada "lineas" y lo que se hace es reescribir esa lista en el txt para que ya quede con los cambios del usuario logueado
            File.WriteAllLines(ArchivoUsuarios, lineas);
        }

        /// <summary>
        /// Guarda el movimiento en archivo plano individual
        /// </summary>
        private void GuardarMovimiento(string movimiento)
        {
            string archivoMovimientos = Path.Combine(CarpetaMovimientos, $"{usuarioActual.NumeroDocumento}.txt");
            using (StreamWriter sw = File.AppendText(archivoMovimientos))
            {
                sw.WriteLine($"{DateTime.Now,-20:dd/MM/yyyy HH:mm} | {movimiento}");
            }
        }

        /// <summary>
        /// Carga los últimos 5 movimientos desde el archivo correspondiente
        /// </summary>
        private void CargarMovimientos()
        {
            string archivoMovimientos = Path.Combine(CarpetaMovimientos, $"{usuarioActual.NumeroDocumento}.txt");
            if (File.Exists(archivoMovimientos))
            {
                var lineas = File.ReadAllLines(archivoMovimientos);
                int start = Math.Max(0, lineas.Length - 5);
                for (int i = start; i < lineas.Length; i++)
                {
                    usuarioActual.Movimientos.Add(lineas[i]);
                }
            }
        }
    }
}