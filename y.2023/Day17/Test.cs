namespace Day17;

public class Test
{
    public static void Start()
    {
        // Crear una matriz de dos dimensiones
        int[,] matriz = new int[5, 5];

        // Inicializar la matriz
        matriz[0, 0] = 0;
        matriz[0, 1] = 1;
        matriz[0, 2] = 4;
        matriz[0, 3] = 0;
        matriz[0, 4] = 0;

        matriz[1, 0] = 1;
        matriz[1, 1] = 0;
        matriz[1, 2] = 0;
        matriz[1, 3] = 1;
        matriz[1, 4] = 0;

        matriz[2, 0] = 4;
        matriz[2, 1] = 0;
        matriz[2, 2] = 0;
        matriz[2, 3] = 0;
        matriz[2, 4] = 1;

        matriz[3, 0] = 0;
        matriz[3, 1] = 1;
        matriz[3, 2] = 0;
        matriz[3, 3] = 0;
        matriz[3, 4] = 1;

        matriz[4, 0] = 0;
        matriz[4, 1] = 0;
        matriz[4, 2] = 1;
        matriz[4, 3] = 1;
        matriz[4, 4] = 0;

        // Inicializar el algoritmo de Dijkstra's
        Dictionary<int, int> distancias = new Dictionary<int, int>();
        List<int> visitados = new List<int>();

        // Empezar desde la esquina superior izquierda
        distancias[0] = 0;
        visitados.Add(0);

        // Iterar sobre todos los nodos
        while (visitados.Count < matriz.GetLength(0))
        {
            // Encontrar el nodo con la distancia más corta
            int nodoActual = distancias.Values.Min();

            // Visitar el nodo actual
            visitados.Add(nodoActual);

            // Actualizar las distancias de los nodos adyacentes
            foreach (int nodoAdyacente in GetNodosAdyacentes(matriz, nodoActual))
            {
                if (!visitados.Contains(nodoAdyacente))
                {
                    int distanciaActual = distancias[nodoActual] + matriz[nodoActual, nodoAdyacente];

                    if (distancias.ContainsKey(nodoAdyacente) && distanciaActual < distancias[nodoAdyacente])
                    {
                        distancias[nodoAdyacente] = distanciaActual;
                    }
                    else if (!distancias.ContainsKey(nodoAdyacente))
                    {
                        distancias[nodoAdyacente] = distanciaActual;
                    }
                }
            }
        }

        // Imprimir las distancias
        foreach (int nodo in distancias.Keys)
        {
            Console.WriteLine("{0}: {1}", nodo, distancias[nodo]);
        }
    }

    // Obtener los nodos adyacentes a un nodo
    static List<int> GetNodosAdyacentes(int[,] matriz, int nodo)
    {
        List<int> nodosAdyacentes = new List<int>();

        for (int i = 0; i < matriz.GetLength(0); i++)
        {
            if (matriz[nodo, i] != 0)
            {
                nodosAdyacentes.Add(i);
            }
        }

        return nodosAdyacentes;
    }
}
