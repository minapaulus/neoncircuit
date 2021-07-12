using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Kruskal
{

    public static List<Edge> GetMinimumSpanningTree(List<Edge> edges, List<int> vertices)
    {
        List<Edge> result = new List<Edge>();

        // if out of bounds increase this
        DisjointSet set = new DisjointSet(1000);
        foreach (int vertex in vertices)
            set.MakeSet(vertex);

        var sortedEdge = edges.OrderBy(x => x.Weight).ToList();

        foreach (Edge edge in sortedEdge)
        {
            if (set.FindSet(edge.Vertex1) != set.FindSet(edge.Vertex2))
            {
                result.Add(edge);
                set.Union(edge.Vertex1, edge.Vertex2);
            }
        }
        return result;
    }
}
