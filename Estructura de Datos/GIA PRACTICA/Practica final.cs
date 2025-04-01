using System;
using System.Collections.Generic;
using System.Linq;

namespace BuscadorVuelosGrafos
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

    // Clase que representa un grafo dirigido ponderado
    public class GrafoVuelos
    {
        // Diccionario que mapea cada ciudad a sus conexiones (destino, precio)
        private Dictionary<string, List<(string destino, decimal precio)>> _adyacencias;
        
        // Lista de todos los vuelos
        private List<Vuelo> _vuelos;

        public GrafoVuelos()
        {
            _adyacencias = new Dictionary<string, List<(string, decimal)>>();
            _vuelos = new List<Vuelo>();
        }

        // Agregar un vuelo al grafo
        public void AgregarVuelo(Vuelo vuelo)
        {
            _vuelos.Add(vuelo);
            
            // Agregar el origen al diccionario si no existe
            if (!_adyacencias.ContainsKey(vuelo.Origen))
            {
                _adyacencias[vuelo.Origen] = new List<(string, decimal)>();
            }
            
            // Agregar el destino como adyacente al origen
            _adyacencias[vuelo.Origen].Add((vuelo.Destino, vuelo.Precio));
        }

        // Obtener todas las ciudades (nodos) del grafo
        public List<string> ObtenerCiudades()
        {
            HashSet<string> ciudades = new HashSet<string>();
            
            // Agregar todos los orígenes
            foreach (var origen in _adyacencias.Keys)
            {
                ciudades.Add(origen);
            }
            
            // Agregar todos los destinos
            foreach (var adyacentes in _adyacencias.Values)
            {
                foreach (var (destino, _) in adyacentes)
                {
                    ciudades.Add(destino);
                }
            }
            
            return ciudades.ToList();
        }

        // Obtener todos los vuelos
        public List<Vuelo> ObtenerVuelos()
        {
            return _vuelos;
        }

        // Buscar la ruta más barata entre dos ciudades usando el algoritmo de Dijkstra
        public (List<string> ruta, decimal precioTotal) EncontrarRutaMasBarata(string origen, string destino)
        {
            // Validar que origen y destino existan
            List<string> ciudades = ObtenerCiudades();
            if (!ciudades.Contains(origen) || !ciudades.Contains(destino))
            {
                return (new List<string>(), 0); // Ruta vacía si origen o destino no existen
            }

            // Inicializar diccionarios para el algoritmo de Dijkstra
            Dictionary<string, decimal> distancias = new Dictionary<string, decimal>();
            Dictionary<string, string> predecesores = new Dictionary<string, string>();
            HashSet<string> visitados = new HashSet<string>();

            // Inicializar distancias a "infinito" para todas las ciudades excepto el origen
            foreach (var ciudad in ciudades)
            {
                distancias[ciudad] = ciudad == origen ? 0 : decimal.MaxValue;
                predecesores[ciudad] = null;
            }

            // Implementación del algoritmo de Dijkstra
            while (visitados.Count < ciudades.Count)
            {
                // Encontrar la ciudad no visitada con la menor distancia
                string actual = null;
                decimal distanciaMinima = decimal.MaxValue;
                
                foreach (var ciudad in ciudades)
                {
                    if (!visitados.Contains(ciudad) && distancias[ciudad] < distanciaMinima)
                    {
                        actual = ciudad;
                        distanciaMinima = distancias[ciudad];
                    }
                }

                // Si no hay más ciudades accesibles o ya llegamos al destino, terminar
                if (actual == null || actual == destino)
                    break;

                visitados.Add(actual);

                // Si la ciudad actual tiene conexiones
                if (_adyacencias.ContainsKey(actual))
                {
                    // Actualizar distancias para los vecinos no visitados
                    foreach (var (vecino, precio) in _adyacencias[actual])
                    {
                        if (!visitados.Contains(vecino))
                        {
                            decimal nuevaDistancia = distancias[actual] + precio;
                            
                            if (nuevaDistancia < distancias[vecino])
                            {
                                distancias[vecino] = nuevaDistancia;
                                predecesores[vecino] = actual;
                            }
                        }
                    }
                }
            }

            // Reconstruir la ruta
            List<string> ruta = new List<string>();
            string ciudadActual = destino;
            
            // Si no hay ruta, devolver lista vacía
            if (predecesores[destino] == null && destino != origen)
            {
                return (new List<string>(), 0);
            }

            // Reconstruir la ruta desde el destino hasta el origen
            while (ciudadActual != null)
            {
                ruta.Add(ciudadActual);
                ciudadActual = predecesores[ciudadActual];
            }
            
            // Invertir la ruta para que vaya desde el origen hasta el destino
            ruta.Reverse();
            
            return (ruta, distancias[destino]);
        }

        // Obtener los vuelos específicos para una ruta
        public List<Vuelo> ObtenerVuelosDeRuta(List<string> ruta)
        {
            List<Vuelo> vuelosRuta = new List<Vuelo>();
            
            for (int i = 0; i < ruta.Count - 1; i++)
            {
                string origen = ruta[i];
                string destino = ruta[i + 1];
                
                // Encontrar el vuelo más barato entre estas dos ciudades
                Vuelo vueloMasBarato = _vuelos
                    .Where(v => v.Origen == origen && v.Destino == destino)
                    .OrderBy(v => v.Precio)
                    .FirstOrDefault();
                
                if (vueloMasBarato != null)
                {
                    vuelosRuta.Add(vueloMasBarato);
                }
            }
            
            return vuelosRuta;
        }
    }

    // Simulador de base de datos (en memoria)
    public class VuelosData
    {
        // Crear el grafo de vuelos con datos de ejemplo
        public static GrafoVuelos CrearGrafoVuelos()
        {
            GrafoVuelos grafo = new GrafoVuelos();
            
            // Agregar vuelos al grafo
            grafo.AgregarVuelo(new Vuelo { Origen = "Madrid", Destino = "Barcelona", Fecha = DateTime.Parse("10/04/2025 08:00"), Aerolinea = "Iberia", Precio = 85.50m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Barcelona", Destino = "Madrid", Fecha = DateTime.Parse("10/04/2025 12:00"), Aerolinea = "Iberia", Precio = 92.30m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "Madrid", Destino = "París", Fecha = DateTime.Parse("12/04/2025 10:15"), Aerolinea = "Air France", Precio = 142.75m });
            grafo.AgregarVuelo(new Vuelo { Origen = "París", Destino = "Madrid", Fecha = DateTime.Parse("15/04/2025 16:30"), Aerolinea = "Air France", Precio = 156.80m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "Barcelona", Destino = "Londres", Fecha = DateTime.Parse("15/04/2025 14:30"), Aerolinea = "British Airways", Precio = 178.30m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Londres", Destino = "Barcelona", Fecha = DateTime.Parse("18/04/2025 10:00"), Aerolinea = "British Airways", Precio = 185.50m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "Madrid", Destino = "Berlín", Fecha = DateTime.Parse("02/04/2025 07:45"), Aerolinea = "Lufthansa", Precio = 165.00m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Berlín", Destino = "Madrid", Fecha = DateTime.Parse("05/04/2025 11:30"), Aerolinea = "Lufthansa", Precio = 172.40m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "Barcelona", Destino = "Roma", Fecha = DateTime.Parse("08/04/2025 16:00"), Aerolinea = "Alitalia", Precio = 110.25m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Roma", Destino = "Barcelona", Fecha = DateTime.Parse("12/04/2025 08:45"), Aerolinea = "Alitalia", Precio = 118.60m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "Madrid", Destino = "Lisboa", Fecha = DateTime.Parse("05/04/2025 09:30"), Aerolinea = "TAP", Precio = 75.80m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Lisboa", Destino = "Madrid", Fecha = DateTime.Parse("07/04/2025 18:15"), Aerolinea = "TAP", Precio = 82.40m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "París", Destino = "Londres", Fecha = DateTime.Parse("14/04/2025 09:00"), Aerolinea = "Air France", Precio = 125.30m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Londres", Destino = "París", Fecha = DateTime.Parse("16/04/2025 11:45"), Aerolinea = "British Airways", Precio = 132.70m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "París", Destino = "Roma", Fecha = DateTime.Parse("18/04/2025 13:20"), Aerolinea = "Air France", Precio = 155.90m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Roma", Destino = "París", Fecha = DateTime.Parse("22/04/2025 15:10"), Aerolinea = "Alitalia", Precio = 162.40m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "Londres", Destino = "Berlín", Fecha = DateTime.Parse("20/04/2025 12:00"), Aerolinea = "British Airways", Precio = 145.60m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Berlín", Destino = "Londres", Fecha = DateTime.Parse("23/04/2025 14:30"), Aerolinea = "Lufthansa", Precio = 152.90m });
            
            grafo.AgregarVuelo(new Vuelo { Origen = "Lisboa", Destino = "Barcelona", Fecha = DateTime.Parse("10/04/2025 07:30"), Aerolinea = "TAP", Precio = 120.40m });
            grafo.AgregarVuelo(new Vuelo { Origen = "Barcelona", Destino = "Lisboa", Fecha = DateTime.Parse("13/04/2025 17:00"), Aerolinea = "Iberia", Precio = 128.70m });
            
            return grafo;
        }
    }

    // Buscador de vuelos con grafos
    public class BuscadorVuelosGrafos
    {
        private GrafoVuelos _grafo;

        public BuscadorVuelosGrafos()
        {
            // Crear el grafo con los datos de vuelos
            _grafo = VuelosData.CrearGrafoVuelos();
        }

        // Obtener lista de ciudades disponibles
        public List<string> ObtenerCiudades()
        {
            return _grafo.ObtenerCiudades();
        }

        // Buscar vuelos directos
        public List<Vuelo> BuscarVuelosDirectos(string origen, string destino)
        {
            return _grafo.ObtenerVuelos()
                .Where(v => v.Origen == origen && v.Destino == destino)
                .OrderBy(v => v.Precio)
                .ToList();
        }

        // Buscar la ruta más barata entre dos ciudades (posiblemente con escalas)
        public (List<string> ciudades, List<Vuelo> vuelos, decimal precioTotal) EncontrarRutaMasBarata(string origen, string destino)
        {
            var (ruta, precio) = _grafo.EncontrarRutaMasBarata(origen, destino);
            List<Vuelo> vuelos = _grafo.ObtenerVuelosDeRuta(ruta);
            
            return (ruta, vuelos, precio);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== BUSCADOR DE VUELOS BARATOS CON GRAFOS ===\n");
            
            BuscadorVuelosGrafos buscador = new BuscadorVuelosGrafos();
            
            while (true)
            {
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Ver ciudades disponibles");
                Console.WriteLine("2. Buscar vuelos directos");
                Console.WriteLine("3. Encontrar ruta más barata (con posibles escalas)");
                Console.WriteLine("4. Salir");
                Console.Write("> ");
                
                string opcion = Console.ReadLine();
                
                switch (opcion)
                {
                    case "1":
                        MostrarCiudades(buscador);
                        break;
                    case "2":
                        BuscarVuelosDirectos(buscador);
                        break;
                    case "3":
                        EncontrarRutaMasBarata(buscador);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.\n");
                        break;
                }
            }
        }
        
        static void MostrarCiudades(BuscadorVuelosGrafos buscador)
        {
            List<string> ciudades = buscador.ObtenerCiudades();
            
            Console.WriteLine("\nCiudades disponibles en el sistema:");
            foreach (var ciudad in ciudades)
            {
                Console.WriteLine($"- {ciudad}");
            }
            Console.WriteLine();
        }
        
        static void BuscarVuelosDirectos(BuscadorVuelosGrafos buscador)
        {
            Console.Write("\nCiudad de origen: ");
            string origen = Console.ReadLine();
            
            Console.Write("Ciudad de destino: ");
            string destino = Console.ReadLine();
            
            var vuelos = buscador.BuscarVuelosDirectos(origen, destino);
            
            Console.WriteLine($"\nVuelos directos de {origen} a {destino}:");
            if (vuelos.Count == 0)
            {
                Console.WriteLine("No se encontraron vuelos directos para la ruta especificada.");
            }
            else
            {
                foreach (var vuelo in vuelos)
                {
                    Console.WriteLine(vuelo);
                }
            }
            Console.WriteLine();
        }
        
        static void EncontrarRutaMasBarata(BuscadorVuelosGrafos buscador)
        {
            Console.Write("\nCiudad de origen: ");
            string origen = Console.ReadLine();
            
            Console.Write("Ciudad de destino: ");
            string destino = Console.ReadLine();
            
            var (ciudades, vuelos, precioTotal) = buscador.EncontrarRutaMasBarata(origen, destino);
            
            if (ciudades.Count <= 1)
            {
                Console.WriteLine($"\nNo se encontró ninguna ruta entre {origen} y {destino}.");
                Console.WriteLine();
                return;
            }
            
            Console.WriteLine($"\nRuta más barata de {origen} a {destino}:");
            Console.WriteLine($"Precio total: ${precioTotal:F2}");
            Console.WriteLine("Itinerario:");
            
            Console.WriteLine($"Ruta: {string.Join(" -> ", ciudades)}");
            Console.WriteLine("\nVuelos:");
            
            foreach (var vuelo in vuelos)
            {
                Console.WriteLine(vuelo);
            }
            
            Console.WriteLine();
        }
    }
}