using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Crear una lista para almacenar el abecedario
        List<char> abecedario = new List<char>();

        // Llenar la lista con el abecedario (26 letras)
        for (char letra = 'A'; letra <= 'Z'; letra++)
        {
            abecedario.Add(letra);
        }

        // Mostrar el abecedario original
        Console.WriteLine("Abecedario original:");
        Console.WriteLine("-------------------");
        MostrarLista(abecedario);

        // Crear una lista para almacenar las posiciones a eliminar
        List<int> posicionesAEliminar = new List<int>();

        // Identificar las posiciones múltiplos de 3
        for (int i = 2; i < abecedario.Count; i += 3)
        {
            posicionesAEliminar.Add(i);
        }

        // Eliminar las letras de atrás hacia adelante para no afectar los índices
        posicionesAEliminar.Reverse();
        foreach (int posicion in posicionesAEliminar)
        {
            abecedario.RemoveAt(posicion);
        }

        // Mostrar el resultado
        Console.WriteLine("\nAbecedario sin letras en posiciones múltiplos de 3:");
        Console.WriteLine("------------------------------------------------");
        MostrarLista(abecedario);

        Console.WriteLine("\nPresione cualquier tecla para salir...");
        Console.ReadKey();
    }

    // Método para mostrar la lista de caracteres
    static void MostrarLista(List<char> lista)
    {
        foreach (char letra in lista)
        {
            Console.Write(letra + " ");
        }
        Console.WriteLine();
    }
}