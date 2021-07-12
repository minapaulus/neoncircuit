using System.Collections;
using System.Collections.Generic;

public class Edge
{
    public int Vertex1 { get; set; }
    public int Vertex2 { get; set; }
    public int Weight { get; set; }

    public Edge(int v1, int v2, int weight)
    {
        Vertex1 = v1;
        Vertex2 = v2;
        Weight = weight;
    }
}