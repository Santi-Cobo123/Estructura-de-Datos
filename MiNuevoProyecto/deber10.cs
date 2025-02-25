using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using VaccinationAnalysis;
using System.Collections.Generic;   


namespace VaccinationAnalysis
{
    public class Ciudadano
    {
        public int id {get;set;}
        public string nombre {get;set;}
        public bool VacunadoPfizer {get;set;}
        public bool VacunadoAstrazeneca {get;set;}


        public bool EstaVacunado=>VacunadoPfizer||VacunadoAstrazeneca;
        public bool TieneDosVacunas=>VacunadoPfizer&&VacunadoAstrazeneca;
     
    }

    public class GeneradorDatos
    {
        private Random random;
        public GeneradorDatos()
        {
            random=new object();
        }

        public list<Ciudadano> GeneradorDatos(int cantidad)
        {
            var Ciudadanos=new list<Ciudadano>();
            for(int i=1;i<=cabtidad;i++)
            {
                Ciudadano.add(new Ciudadano
                {
                    id=i,
                    nombre=$"Ciudadano{i}"
                });
            }
            return Ciudadanos;
        }

        public void AsignarVacunas(list<Ciudadano> Ciudadanos,int cantidadPfizer,int cantidadAstrazeneca)
        {
            var indices=Enumerable.Range(0,Ciudadanos.Count).Tolist();

            //Asignar Pfizer
            for(int i=0;i<cantidadPfizer;i++)
            {
                int index=random.Next(indices.Count);
                Ciudadanos[indices[index]].VacunadoPfizer=true;
                indices.RemoveAt(index);
            }

            // Resetear indices para Astrazeneca
            indices=ciudadanos.Where(c=>!c.VacunadoPfizer)
                    .Select(c=>Ciudadanos.IndexOf(c))
                    .Tolist();
            
            //Asignar Astrazeneca
            for(int i=0;i<cantidadAstrazeneca;i++)
            {
                if(indices.Cont==0)break;
                int index=random.Next(indices.Count);
                Ciudadanos[indices[index]].VacunadoAstrazeneca=true;
                indices.RemoveAt(index);
            }
        }
    }

    public class AnalizadorVacunacion
    {
        public void GeneradorReporte(list<Ciudadano> ciudadanos,string rutaArchivo)
        {
            //No vacunados (Complemento del conjunto de vacunados)
            var noVacunados=ciudadanos.Where(c=> !c.EstaVacunado).Tolist();
            writer.WriteLine("=== CIUDADANOS NO VACUNADOS ===");
            noVacunados.ForEach(c=>writer.WriteLine($"ID:{c.id},Nombre:{c.Nombre}"));
            writer.WriteLine($"Total no vacunados:{noVacunados.Count}\n");

            // Con ambas vacunas (Intersección de conjuntos)
            var dosVacunas=ciudadanos.Where(c=>c.TieneDosVacunas).Tolist();
            writer.WriteLine("=== CUIDADANOS CON AMBAS VACUNAS ===");
            dosVacunas.ForEach(c=>writer.WriteLine($"ID:{c.id},Nombre:{c.Nombre}"));
            writer.WriteLine($"Total con dos vacunas:{dosVacunas.Count}\n");

            // Solo Pfizer (Diferencia de conjuntos)
            var soloPfizer=ciudadanos.Where(c=>c.VacunadoPfizer&&!c.VacunadoAstrazeneca).Tolist();
            writer.WriteLine("=== CUIDADANOS SOLO CON PFIZER ===");
            soloPfizer.ForEach(c=>writer.WriteLine($"ID:{c.id},Nombre:{c.Nombre}"));
            writer.WriteLine($"Total solo Pfizer:{soloPfizer.Count}\n");

            // Solo Astrazeneca (Diferencia de conjuntos)
            var soloAstrazeneca=ciudadanos.Where(c=>!c.VacunadoPfizerc.VacunadoAstrazeneca).Tolist();
            writer.WriteLine("=== CIUDADANOS SOLO CON ASTRAZENECA ===");
            soloAstrazeneca.ForEach(c=>writer.WriteLine($"ID:{c.id},Nombre:{c.Nombre}"));
            writer.WriteLine($"Total solo Astrazeneca:{soloAstrazeneca.Count}\n");

        }
    }

    class Program
    {
        static void Main(string[]args)
        {
            //Configuración
            const int TOTAL_CIUDADANOS=500;
            const int VACUNADOS_PFIZER=75;
            const int VACUNADOS_ASTRAZENECA=75;

            //Generar datos
            var generador=new GeneradorDatos();
            var ciudadanos=generador.GeneradorDatos(TOTAL_CIUDADANOS);
            generador.AsignarVacunas(ciudadanos,VACUNADOS_PFIZER,VACUNADOS_ASTRAZENECA);

            //Analizar y generar reporte
            var analizador=new AnalizadorVacunacion();
            string rutaReporte="reporte_vacunacion.txt";
            analizador.GeneradorReporte(ciudadanos,rutaReporte);
            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}