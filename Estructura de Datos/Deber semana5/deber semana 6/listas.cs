using System;

public class Lista
{
    private int[] elementos;
    private int tamaño;
    private const int CAPACIDAD_INICIAL = 10;

    public Lista()
    {
        elementos = new int[CAPACIDAD_INICIAL];
        tamaño = 0;
    }

    // Método para agregar elementos a la lista
    public void Agregar(int valor)
    {
        if (tamaño == elementos.Length)
        {
            Array.Resize(ref elementos, elementos.Length * 2);
        }
        elementos[tamaño] = valor;
        tamaño++;
    }

    // Método de búsqueda que cuenta ocurrencias
    public void Buscar(int valorBuscado)
    {
        int contador = 0;
        
        // Recorremos la lista buscando coincidencias
        for (int i = 0; i < tamaño; i++)
        {
            if (elementos[i] == valorBuscado)
            {
                contador++;
            }
        }
        
        // Mostramos el resultado
        if (contador > 0)
        {
            Console.WriteLine($"El valor {valorBuscado} se encontró {contador} vez(ces) en la lista.");
        }
        else
        {
            Console.WriteLine($"El valor {valorBuscado} no fue encontrado en la lista.");
        }
    }

    // Método para mostrar la lista
    public void MostrarLista()
    {
        if (tamaño == 0)
        {
            Console.WriteLine("La lista está vacía");
            return;
        }

        Console.WriteLine("Elementos en la lista:");
        for (int i = 0; i < tamaño; i++)
        {
            Console.Write(elementos[i] + " ");
        }
        Console.WriteLine();
    }
}

// Programa principal para probar la implementación
class Program
{
    static void Main(string[] args)
    {
        Lista miLista = new Lista();
        
        // Agregamos algunos elementos de prueba
        miLista.Agregar(5);
        miLista.Agregar(3);
        miLista.Agregar(5);
        miLista.Agregar(7);
        miLista.Agregar(5);
        
        // Mostramos la lista
        miLista.MostrarLista();
        
        // Probamos el método de búsqueda
        Console.WriteLine("\nBuscando el valor 5:");
        miLista.Buscar(5);
        
        Console.WriteLine("\nBuscando el valor 2:");
        miLista.Buscar(2);
        
        Console.ReadLine();
    }
}