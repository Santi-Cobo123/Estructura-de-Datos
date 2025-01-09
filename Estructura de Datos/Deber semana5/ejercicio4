using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Crear una lista para almacenar los números
        List<int> numeros = new List<int>();

        // Almacenar los números del 1 al 10 en la lista
        for (int i = 1; i <= 10; i++)
        {
            numeros.Add(i);
        }

        // Mostrar título
        Console.WriteLine("Números del 1 al 10 en orden inverso:");
        Console.WriteLine("-----------------------------------");

        // Mostrar los números en orden inverso
        for (int i = numeros.Count - 1; i >= 0; i--)
        {
            // Si es el último número, no ponemos coma al final
            if (i == 0)
            {
                Console.Write(numeros[i]);
            }
            else
            {
                Console.Write(numeros[i] + ", ");
            }
        }

        // Agregar una línea en blanco al final
        Console.WriteLine("\n\nPresione cualquier tecla para salir...");
        Console.ReadKey();
    }
}