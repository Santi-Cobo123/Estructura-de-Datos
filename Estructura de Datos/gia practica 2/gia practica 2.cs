class Program
{
    static void Main(string[] args)
    {
        AtraccionParque atraccion = new AtraccionParque(30); // Crear atracción con 30 asientos
        
        // Simular llegada de personas
        for (int i = 1; i <= 35; i++) // Intentamos con 35 personas para ver qué pasa cuando se llenan los asientos
        {
            Persona persona = new Persona($"Persona_{i}");
            atraccion.AgregarALaCola(persona);
        }
        
        // Asignar asientos
        atraccion.AsignarAsientos();
        
        // Mostrar asignaciones
        atraccion.MostrarAsignaciones();
    }
}

class Persona
{
    public string Nombre { get; private set; }
    public int NumeroAsiento { get; set; }
    
    public Persona(string nombre)
    {
        Nombre = nombre;
        NumeroAsiento = -1; // -1 indica que aún no tiene asiento asignado
    }
}

class AtraccionParque
{
    private int capacidadMaxima;
    private Queue<Persona> colaEspera;
    private List<Persona> asientosAsignados;
    
    public AtraccionParque(int capacidad)
    {
        capacidadMaxima = capacidad;
        colaEspera = new Queue<Persona>();
        asientosAsignados = new List<Persona>();
    }
    
    public void AgregarALaCola(Persona persona)
    {
        colaEspera.Enqueue(persona);
        Console.WriteLine($"{persona.Nombre} se ha unido a la cola.");
    }
    
    public void AsignarAsientos()
    {
        int numeroAsiento = 1;
        
        while (colaEspera.Count > 0 && numeroAsiento <= capacidadMaxima)
        {
            Persona persona = colaEspera.Dequeue();
            persona.NumeroAsiento = numeroAsiento;
            asientosAsignados.Add(persona);
            numeroAsiento++;
        }
        
        // Informar sobre personas que no alcanzaron asiento
        while (colaEspera.Count > 0)
        {
            Persona persona = colaEspera.Dequeue();
            Console.WriteLine($"Lo sentimos, {persona.Nombre} no alcanzó asiento en esta ronda.");
        }
    }
    
    public void MostrarAsignaciones()
    {
        Console.WriteLine("\nAsignaciones de asientos:");
        foreach (var persona in asientosAsignados)
        {
            Console.WriteLine($"{persona.Nombre} - Asiento #{persona.NumeroAsiento}");
        }
    }
}