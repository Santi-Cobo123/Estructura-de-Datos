using System;
using System.Collections.Generic;

namespace CatalogoRevistas
{
    class Program
    {
        static List<string> catalogoRevistas = new List<string>
        {
            "Moda al Dia",
            "Accesorios unicos",
            "Hogar Elegante",
            "Belleza Natural",
            "Tecnologia Top",
            "Viajes Inolvidables",
            "Fitness Total",
            "Joyas Brillantes",
            "Regalo Especial",
            "Recetas del mundo"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("=== CATÁLOGO DE REVISTAS ===");
            Console.WriteLine("Revistas disponibles en el catálogo:");
            MostrarCatalogo();

            int opcion;
            do
            {
                Console.WriteLine("\nMenú de opciones:");
                Console.WriteLine("1. Buscar título (método recursivo)");
                Console.WriteLine("2. Buscar título (método iterativo)");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            BuscarTituloRecursivo();
                            break;
                        case 2:
                            BuscarTituloIterativo();
                            break;
                        case 0:
                            Console.WriteLine("Gracias por usar el catálogo. ¡Hasta pronto!");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente nuevamente.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Por favor ingrese un número.");
                    opcion = -1;
                }
            } while (opcion != 0);
        }

        static void MostrarCatalogo()
        {
            // Ordenamos el catálogo para facilitar la búsqueda
            catalogoRevistas.Sort();
            
            for (int i = 0; i < catalogoRevistas.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {catalogoRevistas[i]}");
            }
        }

        static void BuscarTituloRecursivo()
        {
            Console.Write("\nIngrese el título a buscar: ");
            string titulo = Console.ReadLine();
            
            // Ordenamos el catálogo para aplicar búsqueda binaria
            catalogoRevistas.Sort();
            
            bool encontrado = BusquedaBinariaRecursiva(catalogoRevistas, titulo, 0, catalogoRevistas.Count - 1);
            
            Console.WriteLine(encontrado ? "Resultado: Encontrado" : "Resultado: No encontrado");
        }

        static bool BusquedaBinariaRecursiva(List<string> lista, string objetivo, int inicio, int fin)
        {
            // Caso base: si el rango de búsqueda se agota sin encontrar el elemento
            if (inicio > fin)
                return false;

            int medio = inicio + (fin - inicio) / 2;
            int comparacion = string.Compare(lista[medio], objetivo, StringComparison.OrdinalIgnoreCase);

            // Caso base: elemento encontrado
            if (comparacion == 0)
                return true;

            // Búsqueda en la mitad izquierda
            if (comparacion > 0)
                return BusquedaBinariaRecursiva(lista, objetivo, inicio, medio - 1);

            // Búsqueda en la mitad derecha
            return BusquedaBinariaRecursiva(lista, objetivo, medio + 1, fin);
        }

        static void BuscarTituloIterativo()
        {
            Console.Write("\nIngrese el título a buscar: ");
            string titulo = Console.ReadLine();
            
            // Ordenamos el catálogo para aplicar búsqueda binaria
            catalogoRevistas.Sort();
            
            bool encontrado = BusquedaBinariaIterativa(catalogoRevistas, titulo);
            
            Console.WriteLine(encontrado ? "Resultado: Encontrado" : "Resultado: No encontrado");
        }

        static bool BusquedaBinariaIterativa(List<string> lista, string objetivo)
        {
            int inicio = 0;
            int fin = lista.Count - 1;

            while (inicio <= fin)
            {
                int medio = inicio + (fin - inicio) / 2;
                int comparacion = string.Compare(lista[medio], objetivo, StringComparison.OrdinalIgnoreCase);

                // Elemento encontrado
                if (comparacion == 0)
                    return true;

                // Búsqueda en la mitad izquierda
                if (comparacion > 0)
                    fin = medio - 1;
                // Búsqueda en la mitad derecha
                else
                    inicio = medio + 1;
            }

            return false;
        }
    }
}