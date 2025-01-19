
namespace ListaNumeros
{
    class Program
    {
        static void Main(string[] args)
        {
            // Crear lista de números
            List<int> numeros = new List<int>();
            Random random = new Random();

            // Generar 50 números aleatorios
            Console.WriteLine("Generando 50 números aleatorios...");
            for (int i = 0; i < 50; i++)
            {
                numeros.Add(random.Next(1, 1000));
            }

            // Mostrar números originales
            Console.WriteLine("\nNúmeros generados:");
            foreach (int num in numeros)
            {
                Console.Write(num + " ");
            }

            try
            {
                // Pedir rango al usuario
                Console.WriteLine("\n\nIngrese el rango de números a mantener:");
                Console.Write("Valor mínimo: ");
                int minimo = Convert.ToInt32(Console.ReadLine());

                Console.Write("Valor máximo: ");
                int maximo = Convert.ToInt32(Console.ReadLine());

                // Verificar que el rango sea válido
                if (minimo > maximo)
                {
                    Console.WriteLine("Error: El valor mínimo no puede ser mayor que el máximo");
                    return;
                }

                // Eliminar números fuera del rango
                numeros.RemoveAll(numero => numero < minimo || numero > maximo);

                // Mostrar resultado
                Console.WriteLine("\nNúmeros dentro del rango {0} - {1}:", minimo, maximo);
                if (numeros.Count == 0)
                {
                    Console.WriteLine("No quedaron números dentro del rango especificado.");
                }
                else
                {
                    foreach (int num in numeros)
                    {
                        Console.Write(num + " ");
                    }
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Por favor ingrese solo números enteros.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}