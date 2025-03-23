using System;

namespace ArbolBinario
{
    class Nodo
    {
        public int Valor;
        public Nodo Izquierdo, Derecho;
        
        public Nodo(int valor)
        {
            Valor = valor;
            Izquierdo = Derecho = null;
        }
    }
    
    class Arbol
    {
        private Nodo raiz;
        
        // Insertar un valor en el árbol
        public void Insertar(int valor)
        {
            if (raiz == null)
                raiz = new Nodo(valor);
            else
                InsertarRecursivo(raiz, valor);
        }
        
        private void InsertarRecursivo(Nodo nodo, int valor)
        {
            if (valor < nodo.Valor)
            {
                if (nodo.Izquierdo == null)
                    nodo.Izquierdo = new Nodo(valor);
                else
                    InsertarRecursivo(nodo.Izquierdo, valor);
            }
            else if (valor > nodo.Valor)
            {
                if (nodo.Derecho == null)
                    nodo.Derecho = new Nodo(valor);
                else
                    InsertarRecursivo(nodo.Derecho, valor);
            }
        }
        
        // Buscar un valor en el árbol
        public bool Busqueda(int valor)
        {
            return BusquedaRecursiva(raiz, valor);
        }
        
        private bool BusquedaRecursiva(Nodo nodo, int valor)
        {
            if (nodo == null)
                return false;
            if (nodo.Valor == valor)
                return true;
                
            return valor < nodo.Valor 
                ? BusquedaRecursiva(nodo.Izquierdo, valor) 
                : BusquedaRecursiva(nodo.Derecho, valor);
        }
        
        // Recorrido InOrden
        public void InOrden()
        {
            InOrdenRecursivo(raiz);
            Console.WriteLine();
        }
        
        private void InOrdenRecursivo(Nodo nodo)
        {
            if (nodo != null)
            {
                InOrdenRecursivo(nodo.Izquierdo);
                Console.Write(nodo.Valor + " ");
                InOrdenRecursivo(nodo.Derecho);
            }
        }
        
        // Recorrido PostOrden
        public void PostOrden()
        {
            PostOrdenRecursivo(raiz);
            Console.WriteLine();
        }
        
        private void PostOrdenRecursivo(Nodo nodo)
        {
            if (nodo != null)
            {
                PostOrdenRecursivo(nodo.Izquierdo);
                PostOrdenRecursivo(nodo.Derecho);
                Console.Write(nodo.Valor + " ");
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Arbol arbol = new Arbol();
            int opcion;
            
            do
            {
                Console.Clear();
                Console.WriteLine("==== ÁRBOL BINARIO ====");
                Console.WriteLine("1. Insertar");
                Console.WriteLine("2. Búsqueda");
                Console.WriteLine("3. InOrden");
                Console.WriteLine("4. PostOrden");
                Console.WriteLine("5. Salir");
                Console.Write("Opción: ");
                
                int.TryParse(Console.ReadLine(), out opcion);
                
                switch (opcion)
                {
                    case 1:
                        Console.Write("Valor a insertar: ");
                        if (int.TryParse(Console.ReadLine(), out int valor))
                            arbol.Insertar(valor);
                        break;
                    case 2:
                        Console.Write("Valor a buscar: ");
                        if (int.TryParse(Console.ReadLine(), out int buscar))
                        {
                            bool encontrado = arbol.Busqueda(buscar);
                            Console.WriteLine(encontrado ? "Encontrado" : "No encontrado");
                        }
                        break;
                    case 3:
                        Console.WriteLine("Recorrido InOrden:");
                        arbol.InOrden();
                        break;
                    case 4:
                        Console.WriteLine("Recorrido PostOrden:");
                        arbol.PostOrden();
                        break;
                }
                
                if (opcion != 5)
                {
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }
            } while (opcion != 5);
        }
    }
}
