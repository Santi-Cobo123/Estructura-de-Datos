using System;

namespace EstructuraDeDatos
{
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
            Console.WriteLine($"El valor {valorBuscado} aparece {contador} veces en la lista");
        }

        // Método para mostrar la lista
        public void MostrarLista()
        {
            Console.WriteLine("Elementos en la lista:");
            for (int i = 0; i < tamaño; i++)
            {
                Console.WriteLine($"Posición {i}: {elementos[i]}");
            }
        }
    }
}