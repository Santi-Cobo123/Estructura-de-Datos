using System;
using System.Collections.Generic;
using System.Linq;

namespace BuscadorVuelosBaratosSimple
{
    // Clase que representa un vuelo
    public class Vuelo
    {
        public string Origen { get; set; }
        public string Destino { get; set; }
        public DateTime Fecha { get; set; }
        public string Aerolinea { get; set; }
        public decimal Precio { get; set; }

        public override string ToString()
        {
            return $"{Origen} -> {Destino} | {Fecha:dd/MM/yyyy HH:mm} | {Aerolinea} | ${Precio:F2}";
        }
    }

    // Simulador de base de datos (en memoria)
    public class VuelosData
    {
        // Lista de vuelos que simula la base de datos
        public static List<Vuelo> ObtenerVuelos()
        {
            return new List<Vuelo>
            {
                new Vuelo { Origen = "Madrid", Destino = "Barcelona", Fecha = DateTime.Parse("10/04/2025 08:00"), Aerolinea = "Iberia", Precio = 85.50m },
                new Vuelo { Origen = "Madrid", Destino = "París", Fecha = DateTime.Parse("12/04/2025 10:15"), Aerolinea = "Air France", Precio = 142.75m },
                new Vuelo { Origen = "Barcelona", Destino = "Londres", Fecha = DateTime.Parse("15/04/2025 14:30"), Aerolinea = "British Airways", Precio = 178.30m },
                new Vuelo { Origen = "Madrid", Destino = "Berlín", Fecha = DateTime.Parse("02/04/2025 07:45"), Aerolinea = "Lufthansa", Precio = 165.00m },
                new Vuelo { Origen = "Barcelona", Destino = "Roma", Fecha = DateTime.Parse("08/04/2025 16:00"), Aerolinea = "Alitalia", Precio = 110.25m },
                new Vuelo { Origen = "Madrid", Destino = "Lisboa", Fecha = DateTime.Parse("05/04/2025 09:30"), Aerolinea = "TAP", Precio = 75.80m },
                new Vuelo { Origen = "Madrid", Destino = "Amsterdam", Fecha = DateTime.Parse("20/04/2025 12:00"), Aerolinea = "KLM", Precio = 195.45m },
                new Vuelo { Origen = "Barcelona", Destino = "Viena", Fecha = DateTime.Parse("18/04/2025 15:15"), Aerolinea = "Austrian", Precio = 188.90m },
                new Vuelo { Origen = "Madrid", Destino = "Bruselas", Fecha = DateTime.Parse("01/04/2025 08:30"), Aerolinea = "Brussels Airlines", Precio = 89.95m },
                new Vuelo { Origen = "Barcelona", Destino = "Múnich", Fecha = DateTime.Parse("22/04/2025 11:45"), Aerolinea = "Lufthansa", Precio = 152.60m }
            };
        }
    }

    // Buscador de vuelos
    public class BuscadorVuelos
    {
        private List<Vuelo> _vuelos;

        public BuscadorVuelos()
        {
            // Carga los datos de vuelos (simulando la lectura de una BD)
            _vuelos = VuelosData.ObtenerVuelos();
        }

        // Método para buscar vuelos según criterios
        public List<Vuelo> BuscarVuelos(string origen = null, string destino = null, 
                                        decimal? precioMaximo = null, int limite = 5)
        {
            // Filtrar vuelos según los criterios
            var consulta = _vuelos.AsQueryable();

            if (!string.IsNullOrEmpty(origen))
                consulta = consulta.Where(v => v.Origen.Equals(origen, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(destino))
                consulta = consulta.Where(v => v.Destino.Equals(destino, StringComparison.OrdinalIgnoreCase));

            if (precioMaximo.HasValue)
                consulta = consulta.Where(v => v.Precio <= precioMaximo.Value);

            // Ordenar por precio (más baratos primero) y limitar resultados
            return consulta.OrderBy(v => v.Precio).Take(limite).ToList();
        }

        // Método para sugerir destinos económicos desde un origen
        public Dictionary<string, decimal> DestinosEconomicos(string origen, int cantidad = 3)
        {
            return _vuelos
                .Where(v => v.Origen.Equals(origen, StringComparison.OrdinalIgnoreCase))
                .GroupBy(v => v.Destino)
                .Select(g => new { Destino = g.Key, PrecioMinimo = g.Min(v => v.Precio) })
                .OrderBy(d => d.PrecioMinimo)
                .Take(cantidad)
                .ToDictionary(d => d.Destino, d => d.PrecioMinimo);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== BUSCADOR DE VUELOS BARATOS ===\n");
            
            BuscadorVuelos buscador = new BuscadorVuelos();
            
            while (true)
            {
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Buscar vuelos baratos");
                Console.WriteLine("2. Ver destinos económicos");
                Console.WriteLine("3. Salir");
                Console.Write("> ");
                
                string opcion = Console.ReadLine();
                
                switch (opcion)
                {
                    case "1":
                        BuscarVuelosBaratos(buscador);
                        break;
                    case "2":
                        MostrarDestinosEconomicos(buscador);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }
        }
        
        static void BuscarVuelosBaratos(BuscadorVuelos buscador)
        {
            Console.Write("\nCiudad de origen (dejar vacío para todos): ");
            string origen = Console.ReadLine();
            
            Console.Write("Ciudad de destino (dejar vacío para todos): ");
            string destino = Console.ReadLine();
            
            Console.Write("Precio máximo (dejar vacío para ignorar): ");
            string precioStr = Console.ReadLine();
            decimal? precioMaximo = !string.IsNullOrEmpty(precioStr) ? 
                                    decimal.Parse(precioStr) : (decimal?)null;
            
            var resultados = buscador.BuscarVuelos(origen, destino, precioMaximo);
            
            Console.WriteLine("\nResultados encontrados:");
            if (resultados.Count == 0)
            {
                Console.WriteLine("No se encontraron vuelos con los criterios especificados.");
            }
            else
            {
                foreach (var vuelo in resultados)
                {
                    Console.WriteLine(vuelo);
                }
            }
            
            Console.WriteLine();
        }
        
        static void MostrarDestinosEconomicos(BuscadorVuelos buscador)
        {
            Console.Write("\nCiudad de origen: ");
            string origen = Console.ReadLine();
            
            if (string.IsNullOrEmpty(origen))
            {
                Console.WriteLine("Debe especificar una ciudad de origen.\n");
                return;
            }
            
            var destinos = buscador.DestinosEconomicos(origen);
            
            Console.WriteLine($"\nDestinos más económicos desde {origen}:");
            if (destinos.Count == 0)
            {
                Console.WriteLine($"No se encontraron vuelos desde {origen}.");
            }
            else
            {
                foreach (var destino in destinos)
                {
                    Console.WriteLine($"{destino.Key}: ${destino.Value:F2}");
                }
            }
            
            Console.WriteLine();
        }
    }
}