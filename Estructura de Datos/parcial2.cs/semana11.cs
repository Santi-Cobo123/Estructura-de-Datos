using System;
using System.Collections.Generic;

namespace TraductorBasico
{
    class Program
    {
        // Diccionarios para almacenar las palabras en ambos idiomas
        static Dictionary<string, string> espanolIngles = new Dictionary<string, string>();
        static Dictionary<string, string> inglesEspanol = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            // Inicializar los diccionarios con las palabras base
            InicializarDiccionarios();

            int opcion;
            do
            {
                MostrarMenu();
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    switch (opcion)
                    {
                        case 1:
                            TraducirFrase();
                            break;
                        case 2:
                            AgregarPalabra();
                            break;
                        case 0:
                            Console.WriteLine("¡Gracias por usar el traductor! Hasta pronto.");
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente nuevamente.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Por favor, ingrese un número válido.");
                    opcion = -1;
                }

                Console.WriteLine();
            } while (opcion != 0);
        }

        static void MostrarMenu()
        {
            Console.WriteLine("MENU");
            Console.WriteLine("=======================================================");
            Console.WriteLine("1. Traducir una frase");
            Console.WriteLine("2. Ingresar más palabras al diccionario");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void InicializarDiccionarios()
        {
            // Lista inicial de palabras
            AgregarTraduccion("tiempo", "time");
            AgregarTraduccion("persona", "person");
            AgregarTraduccion("año", "year");
            AgregarTraduccion("camino", "way");
            AgregarTraduccion("forma", "way");
            AgregarTraduccion("día", "day");
            AgregarTraduccion("cosa", "thing");
            AgregarTraduccion("hombre", "man");
            AgregarTraduccion("mundo", "world");
            AgregarTraduccion("vida", "life");
            AgregarTraduccion("mano", "hand");
            AgregarTraduccion("parte", "part");
            AgregarTraduccion("niño", "child");
            AgregarTraduccion("niña", "child");
            AgregarTraduccion("ojo", "eye");
            AgregarTraduccion("mujer", "woman");
            AgregarTraduccion("lugar", "place");
            AgregarTraduccion("trabajo", "work");
            AgregarTraduccion("semana", "week");
            AgregarTraduccion("caso", "case");
            AgregarTraduccion("punto", "point");
            AgregarTraduccion("tema", "point");
            AgregarTraduccion("gobierno", "government");
            AgregarTraduccion("empresa", "company");
            AgregarTraduccion("compañía", "company");
        }

        static void AgregarTraduccion(string palabraEspanol, string palabraIngles)
        {
            // Agregar a ambos diccionarios para permitir traducción en ambas direcciones
            espanolIngles[palabraEspanol.ToLower()] = palabraIngles.ToLower();
            inglesEspanol[palabraIngles.ToLower()] = palabraEspanol.ToLower();
        }

        static void TraducirFrase()
        {
            Console.Write("Ingrese la frase: ");
            string frase = Console.ReadLine();
            
            // Verificar si la frase está vacía
            if (string.IsNullOrWhiteSpace(frase))
            {
                Console.WriteLine("Por favor, ingrese una frase válida.");
                return;
            }

            // Dividir la frase en palabras
            string[] palabras = frase.Split(new char[] { ' ', ',', '.', ':', ';', '!', '?' }, 
                                           StringSplitOptions.RemoveEmptyEntries);
            
            // Texto original con puntuación
            string[] caracteres = frase.Split();
            List<string> resultado = new List<string>();
            
            int indicepalabra = 0;
            
            foreach (string caracter in caracteres)
            {
                string palabra = caracter;
                
                // Eliminar puntuación para buscar en el diccionario
                string palabraSinPuntuacion = new string(
                    caracter.Where(c => !char.IsPunctuation(c)).ToArray()
                ).ToLower();
                
                if (!string.IsNullOrWhiteSpace(palabraSinPuntuacion))
                {
                    // Verificar si la palabra existe en español
                    if (espanolIngles.ContainsKey(palabraSinPuntuacion))
                    {
                        // Mantener mayúsculas si la palabra original las tiene
                        if (char.IsUpper(caracter[0]))
                        {
                            string traducida = espanolIngles[palabraSinPuntuacion];
                            palabra = char.ToUpper(traducida[0]) + traducida.Substring(1);
                        }
                        else
                        {
                            palabra = espanolIngles[palabraSinPuntuacion];
                        }
                        
                        // Agregar puntuación original
                        foreach (char c in caracter)
                        {
                            if (char.IsPunctuation(c))
                            {
                                palabra += c;
                            }
                        }
                    }
                    // Verificar si la palabra existe en inglés
                    else if (inglesEspanol.ContainsKey(palabraSinPuntuacion))
                    {
                        // Mantener mayúsculas si la palabra original las tiene
                        if (char.IsUpper(caracter[0]))
                        {
                            string traducida = inglesEspanol[palabraSinPuntuacion];
                            palabra = char.ToUpper(traducida[0]) + traducida.Substring(1);
                        }
                        else
                        {
                            palabra = inglesEspanol[palabraSinPuntuacion];
                        }
                        
                        // Agregar puntuación original
                        foreach (char c in caracter)
                        {
                            if (char.IsPunctuation(c))
                            {
                                palabra += c;
                            }
                        }
                    }
                }
                
                resultado.Add(palabra);
            }
            
            string fraseTraducida = string.Join(" ", resultado);
            Console.WriteLine("Su frase traducida es: " + fraseTraducida);
        }

        static void AgregarPalabra()
        {
            Console.Write("Ingrese la palabra en español: ");
            string palabraEspanol = Console.ReadLine().ToLower();
            
            Console.Write("Ingrese la traducción en inglés: ");
            string palabraIngles = Console.ReadLine().ToLower();
            
            if (string.IsNullOrWhiteSpace(palabraEspanol) || string.IsNullOrWhiteSpace(palabraIngles))
            {
                Console.WriteLine("Por favor, ingrese palabras válidas.");
                return;
            }

            AgregarTraduccion(palabraEspanol, palabraIngles);
            Console.WriteLine($"Palabra '{palabraEspanol}' - '{palabraIngles}' agregada correctamente.");
        }
    }
}