using System;
using System.Collections.Generic;
using System.Linq;

namespace PremiacionDeportistaConciso
{
    enum CategoriaPremio { Oro, Plata, Bronce, MencionHonorifica }

    class Deportista
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        
        public override string ToString() => $"{Id} - {Nombre} ({Pais})";
    }

    class Disciplina
    {
        public string Nombre { get; set; }
        public HashSet<Deportista> Deportistas { get; private set; }
        public Dictionary<CategoriaPremio, HashSet<Deportista>> Premiados { get; private set; }

        public Disciplina(string nombre)
        {
            Nombre = nombre;
            Deportistas = new HashSet<Deportista>(new DeportistaComparer());
            Premiados = new Dictionary<CategoriaPremio, HashSet<Deportista>>();
            
            // Inicializar los conjuntos para cada categoría
            foreach (CategoriaPremio cat in Enum.GetValues(typeof(CategoriaPremio)))
                Premiados[cat] = new HashSet<Deportista>(new DeportistaComparer());
        }

        public bool AgregarDeportista(Deportista deportista) => Deportistas.Add(deportista);

        public bool AsignarPremio(string idDeportista, CategoriaPremio categoria)
        {
            var deportista = Deportistas.FirstOrDefault(d => d.Id == idDeportista);
            if (deportista == null) return false;
            
            // Para oro, plata y bronce solo puede haber un ganador
            if ((int)categoria <= 2 && Premiados[categoria].Count > 0) 
                return false;
                
            return Premiados[categoria].Add(deportista);
        }
    }

    class DeportistaComparer : IEqualityComparer<Deportista>
    {
        public bool Equals(Deportista x, Deportista y) => x.Id == y.Id;
        public int GetHashCode(Deportista obj) => obj.Id.GetHashCode();
    }

    class EventoDeportivo
    {
        public string Nombre { get; set; }
        public Dictionary<string, Disciplina> Disciplinas { get; private set; }

        public EventoDeportivo(string nombre)
        {
            Nombre = nombre;
            Disciplinas = new Dictionary<string, Disciplina>();
        }

        public bool AgregarDisciplina(Disciplina disciplina)
        {
            if (Disciplinas.ContainsKey(disciplina.Nombre)) return false;
            Disciplinas.Add(disciplina.Nombre, disciplina);
            return true;
        }

        // Unión de conjuntos - todos los deportistas
        public HashSet<Deportista> ObtenerTodosLosDeportistas()
        {
            HashSet<Deportista> todos = new HashSet<Deportista>(new DeportistaComparer());
            foreach (var disciplina in Disciplinas.Values)
                todos.UnionWith(disciplina.Deportistas);
            return todos;
        }

        // Deportistas que participan en múltiples disciplinas
        public Dictionary<Deportista, List<string>> ObtenerMultidisciplinarios()
        {
            Dictionary<Deportista, List<string>> resultado = new Dictionary<Deportista, List<string>>(new DeportistaComparer());
            
            // Registrar disciplinas para cada deportista
            foreach (var disciplina in Disciplinas.Values)
            {
                foreach (var deportista in disciplina.Deportistas)
                {
                    if (!resultado.ContainsKey(deportista))
                        resultado[deportista] = new List<string>();
                    resultado[deportista].Add(disciplina.Nombre);
                }
            }
            
            // Filtrar los que están en más de una disciplina
            return resultado.Where(kvp => kvp.Value.Count > 1)
                           .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        // Obtener ganadores por categoría en todas las disciplinas
        public Dictionary<string, List<Deportista>> ObtenerGanadores(CategoriaPremio categoria)
        {
            Dictionary<string, List<Deportista>> ganadores = new Dictionary<string, List<Deportista>>();
            
            foreach (var disciplina in Disciplinas.Values)
            {
                ganadores[disciplina.Nombre] = disciplina.Premiados[categoria].ToList();
            }
            
            return ganadores;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE PREMIACIÓN DE DEPORTISTAS ===");
            var evento = new EventoDeportivo("Juegos Panamericanos 2025");
            
            // Datos de muestra
            var natacion = new Disciplina("Natación");
            var atletismo = new Disciplina("Atletismo");
            
            evento.AgregarDisciplina(natacion);
            evento.AgregarDisciplina(atletismo);
            
            // Deportistas
            var deportista1 = new Deportista { Id = "D001", Nombre = "Ana López", Pais = "México" };
            var deportista2 = new Deportista { Id = "D002", Nombre = "Carlos Ruiz", Pais = "Colombia" };
            var deportista3 = new Deportista { Id = "D003", Nombre = "María García", Pais = "Argentina" };
            var deportista4 = new Deportista { Id = "D004", Nombre = "Juan Pérez", Pais = "Brasil" };
            
            // Agregar deportistas a disciplinas
            natacion.AgregarDeportista(deportista1);
            natacion.AgregarDeportista(deportista2);
            natacion.AgregarDeportista(deportista3);
            
            atletismo.AgregarDeportista(deportista2);  // Participa en ambas disciplinas
            atletismo.AgregarDeportista(deportista4);
            
            // Asignar premios
            natacion.AsignarPremio("D001", CategoriaPremio.Oro);
            natacion.AsignarPremio("D002", CategoriaPremio.Plata);
            natacion.AsignarPremio("D003", CategoriaPremio.Bronce);
            
            atletismo.AsignarPremio("D004", CategoriaPremio.Oro);
            atletismo.AsignarPremio("D002", CategoriaPremio.Plata);

            int opcion;
            do
            {
                Console.WriteLine("\nMenú de Opciones:");
                Console.WriteLine("1. Listar disciplinas");
                Console.WriteLine("2. Listar deportistas de una disciplina");
                Console.WriteLine("3. Listar todos los deportistas");
                Console.WriteLine("4. Listar deportistas multidisciplinarios");
                Console.WriteLine("5. Listar ganadores por categoría");
                Console.WriteLine("0. Salir");
                
                Console.Write("Seleccione una opción: ");
                int.TryParse(Console.ReadLine(), out opcion);

                switch (opcion)
                {
                    case 1:
                        ListarDisciplinas(evento);
                        break;
                    case 2:
                        ListarDeportistasDisciplina(evento);
                        break;
                    case 3:
                        ListarTodosDeportistas(evento);
                        break;
                    case 4:
                        ListarMultidisciplinarios(evento);
                        break;
                    case 5:
                        ListarGanadores(evento);
                        break;
                }
            } while (opcion != 0);
        }

        static void ListarDisciplinas(EventoDeportivo evento)
        {
            Console.WriteLine("\nDisciplinas registradas:");
            foreach (var disc in evento.Disciplinas.Values)
                Console.WriteLine($"{disc.Nombre} ({disc.Deportistas.Count} deportistas)");
        }

        static void ListarDeportistasDisciplina(EventoDeportivo evento)
        {
            Console.Write("Nombre de la disciplina: ");
            string nombre = Console.ReadLine();
            
            if (!evento.Disciplinas.ContainsKey(nombre))
            {
                Console.WriteLine("La disciplina no existe.");
                return;
            }

            Console.WriteLine($"\nDeportistas de {nombre}:");
            foreach (var deportista in evento.Disciplinas[nombre].Deportistas)
                Console.WriteLine(deportista);
        }

        static void ListarTodosDeportistas(EventoDeportivo evento)
        {
            var deportistas = evento.ObtenerTodosLosDeportistas();
            Console.WriteLine("\nTodos los deportistas del evento:");
            foreach (var deportista in deportistas)
                Console.WriteLine(deportista);
        }

        static void ListarMultidisciplinarios(EventoDeportivo evento)
        {
            var multidisciplinarios = evento.ObtenerMultidisciplinarios();
            Console.WriteLine("\nDeportistas multidisciplinarios:");
            foreach (var kvp in multidisciplinarios)
            {
                Console.WriteLine($"{kvp.Key} - Disciplinas: {string.Join(", ", kvp.Value)}");
            }
        }

        static void ListarGanadores(EventoDeportivo evento)
        {
            Console.WriteLine("\nCategorías disponibles:");
            foreach (CategoriaPremio cat in Enum.GetValues(typeof(CategoriaPremio)))
                Console.WriteLine($"{(int)cat}. {cat}");
                
            Console.Write("Seleccione categoría: ");
            if (!int.TryParse(Console.ReadLine(), out int catInt) || !Enum.IsDefined(typeof(CategoriaPremio), catInt))
            {
                Console.WriteLine("Categoría inválida");
                return;
            }
            
            var categoria = (CategoriaPremio)catInt;
            var ganadores = evento.ObtenerGanadores(categoria);
            
            Console.WriteLine($"\nGanadores de {categoria}:");
            foreach (var kvp in ganadores)
            {
                Console.WriteLine($"Disciplina: {kvp.Key}");
                if (kvp.Value.Count == 0)
                    Console.WriteLine("  No hay ganadores en esta categoría");
                else
                    foreach (var deportista in kvp.Value)
                        Console.WriteLine($"  {deportista}");
            }
        }
    }
}