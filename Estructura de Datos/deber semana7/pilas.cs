using System;
using System.Collections.Generic;

public class BalancedExpressionValidator
{
    /// <summary>
    /// Verifica si una expresión matemática tiene paréntesis, corchetes y llaves balanceados.
    /// </summary>
    /// <param name="expression">Expresión matemática a validar</param>
    /// <returns>True si la expresión está balanceada, false en caso contrario</returns>
    public static bool IsBalancedExpression(string expression)
    {
        // Pila para almacenar paréntesis, corchetes y llaves abiertos
        Stack<char> bracketStack = new Stack<char>();

        // Diccionario para mapear corchetes de cierre con sus respectivas aperturas
        Dictionary<char, char> bracketPairs = new Dictionary<char, char>
        {
            { ')', '(' },
            { ']', '[' },
            { '}', '{' }
        };

        foreach (char symbol in expression)
        {
            // Verificar si el símbolo es un paréntesis de apertura
            if ("({[".Contains(symbol))
            {
                bracketStack.Push(symbol);
            }
            // Verificar si el símbolo es un paréntesis de cierre
            else if (")}]".Contains(symbol))
            {
                // Si la pila está vacía o el último paréntesis no coincide, la expresión no está balanceada
                if (bracketStack.Count == 0 || bracketStack.Pop() != bracketPairs[symbol])
                {
                    return false;
                }
            }
        }

        // La expresión está balanceada si la pila está vacía al final
        return bracketStack.Count == 0;
    }

    public class TowersOfHanoi
    {
        /// <summary>
        /// Resuelve el problema de las Torres de Hanoi utilizando recursividad y una pila.
        /// </summary>
        /// <param name="n">Número de discos</param>
        /// <param name="source">Torre de origen</param>
        /// <param name="auxiliary">Torre auxiliar</param>
        /// <param name="destination">Torre de destino</param>
        public static void SolveTowersOfHanoi(int n, string source, string auxiliary, string destination)
        {
            // Pila para rastrear los movimientos de discos
            Stack<(int, string, string, string)> movementStack = new Stack<(int, string, string, string)>();
            movementStack.Push((n, source, auxiliary, destination));

            while (movementStack.Count > 0)
            {
                var (disks, src, aux, dest) = movementStack.Pop();

                if (disks == 1)
                {
                    Console.WriteLine($"Mover disco 1 de {src} a {dest}");
                }
                else if (disks > 1)
                {
                    // Pasos para mover n discos:
                    // 1. Mover n-1 discos de source a auxiliary
                    // 2. Mover el disco más grande de source a destination
                    // 3. Mover n-1 discos de auxiliary a destination
                    movementStack.Push((disks - 1, aux, dest, src));
                    movementStack.Push((1, src, aux, dest));
                    movementStack.Push((disks - 1, src, aux, dest));
                }
            }
        }

        // Método Main para demostrar el uso de los métodos
        public static void Main()
        {
            // Ejemplo de uso para ValidarExpresión
            string expresion1 = "{7+(8*5)-[(9-7)+(4+1)]}";
            string expresion2 = "{7+(8*5)-[9-7)+(4+1)]}";
            
            Console.WriteLine($"Expresión 1 balanceada: {IsBalancedExpression(expresion1)}");
            Console.WriteLine($"Expresión 2 balanceada: {IsBalancedExpression(expresion2)}");

            // Ejemplo de uso para Torres de Hanoi
            Console.WriteLine("\nSolución Torres de Hanoi con 3 discos:");
            SolveTowersOfHanoi(3, "A", "B", "C");
        }
    }
}