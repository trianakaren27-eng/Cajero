using System;
using System.Collections.Generic;

namespace Cajero
{

    /// <summary>
    /// Clase usuario la cual representa a un cliente del cajero
    /// </summary>
    public class Usuario
    {
        public string NumeroDocumento { get; set; }
        public string Clave { get; set; }
        public decimal Saldo { get; set; }
        public List<string> Movimientos { get; set; }

        public Usuario(string documento, string clave, decimal saldo)
        {
            NumeroDocumento = documento;
            Clave = clave;
            Saldo = saldo;
            Movimientos = new List<string>();
        }

        /// <summary>
        /// Encargado de registrar un movimiento en el historial en memoria 
        /// </summary>

        public void RegistrarMovimiento(string descripcion)
        {
            if (Movimientos.Count == 5)
                Movimientos.RemoveAt(0); // mantiene un maximo de 5 movimientos 

            Movimientos.Add($"{DateTime.Now}: {descripcion}");
        }
    }
}
