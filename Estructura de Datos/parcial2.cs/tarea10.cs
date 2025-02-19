using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace VaccinationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            VaccinationSystem system = new VaccinationSystem();
            system.GenerateReport();
            Console.WriteLine("Reporte generado exitosamente!");
            Console.WriteLine("Revise 'vaccination_report.txt' en el directorio de la aplicación.");
            Console.ReadKey();
        }
    }

    public class VaccinationSystem
    {
        private List<Citizen> citizens;
        private List<Citizen> pfizerVaccinated;
        private List<Citizen> astrazenecaVaccinated;

        public VaccinationSystem()
        {
            citizens = new List<Citizen>();
            pfizerVaccinated = new List<Citizen>();
            astrazenecaVaccinated = new List<Citizen>();
            
            GenerateCitizens();
            AssignVaccines();
        }

        private void GenerateCitizens()
        {
            for (int i = 1; i <= 500; i++)
            {
                citizens.Add(new Citizen 
                { 
                    Id = i,
                    Name = $"Ciudadano {i}",
                    Vaccines = new List<string>()
                });
            }
        }

        private void AssignVaccines()
        {
            Random random = new Random();
            List<Citizen> availableCitizens = new List<Citizen>(citizens);

            // Assign Pfizer vaccines
            for (int i = 0; i < 75 && availableCitizens.Count > 0; i++)
            {
                int index = random.Next(availableCitizens.Count);
                Citizen citizen = availableCitizens[index];
                citizen.Vaccines.Add("Pfizer");
                pfizerVaccinated.Add(citizen);
                availableCitizens.RemoveAt(index);
            }

            // Assign AstraZeneca vaccines
            for (int i = 0; i < 75 && availableCitizens.Count > 0; i++)
            {
                int index = random.Next(availableCitizens.Count);
                Citizen citizen = availableCitizens[index];
                citizen.Vaccines.Add("AstraZeneca");
                astrazenecaVaccinated.Add(citizen);
                availableCitizens.RemoveAt(index);
            }

            // Assign second doses randomly
            List<Citizen> vaccinatedCitizens = new List<Citizen>();
            vaccinatedCitizens.AddRange(pfizerVaccinated);
            vaccinatedCitizens.AddRange(astrazenecaVaccinated);

            for (int i = 0; i < 50 && vaccinatedCitizens.Count > 0; i++)
            {
                int index = random.Next(vaccinatedCitizens.Count);
                Citizen citizen = vaccinatedCitizens[index];
                citizen.Vaccines.Add(citizen.Vaccines[0]); // Add same vaccine as first dose
            }
        }

        public List<Citizen> GetUnvaccinatedCitizens()
        {
            return citizens.Where(c => !c.Vaccines.Any()).ToList();
        }

        public List<Citizen> GetFullyVaccinatedCitizens()
        {
            return citizens.Where(c => c.Vaccines.Count == 2).ToList();
        }

        public List<Citizen> GetPfizerOnlyCitizens()
        {
            return pfizerVaccinated.Where(c => c.Vaccines.All(v => v == "Pfizer")).ToList();
        }

        public List<Citizen> GetAstrazenecaOnlyCitizens()
        {
            return astrazenecaVaccinated.Where(c => c.Vaccines.All(v => v == "AstraZeneca")).ToList();
        }

        public void GenerateReport()
        {
            using (StreamWriter file = new StreamWriter("vaccination_report.txt"))
            {
                file.WriteLine("=== REPORTE DE VACUNACIÓN COVID ===\n");

                file.WriteLine("1. Ciudadanos sin vacunar:");
                foreach (var citizen in GetUnvaccinatedCitizens())
                {
                    file.WriteLine(citizen.ToString());
                }

                file.WriteLine("\n2. Ciudadanos con esquema completo:");
                foreach (var citizen in GetFullyVaccinatedCitizens())
                {
                    file.WriteLine(citizen.ToString());
                }

                file.WriteLine("\n3. Ciudadanos vacunados solo con Pfizer:");
                foreach (var citizen in GetPfizerOnlyCitizens())
                {
                    file.WriteLine(citizen.ToString());
                }

                file.WriteLine("\n4. Ciudadanos vacunados solo con AstraZeneca:");
                foreach (var citizen in GetAstrazenecaOnlyCitizens())
                {
                    file.WriteLine(citizen.ToString());
                }
            }
        }
    }

    public class Citizen
    {
        public int Id { get; set; }
        public required string Name { get; set; }
            public required List<string> Vaccines { get; set; }

        public override string ToString()
        {
            return $"Ciudadano {Id}: {Name} - Vacunas: {string.Join(", ", Vaccines)}";
        }
    }
}