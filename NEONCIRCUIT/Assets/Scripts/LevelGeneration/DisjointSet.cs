using System.Collections;
using System.Collections.Generic;

public class DisjointSet
{
    int[] parent;
    int[] rank;

    public DisjointSet(int length)
    {
        parent = new int[length];
        rank = new int[length];

        for (int i = 0; i < parent.Length; i++)
            parent[i] = i;
    }

    public void MakeSet(int x)
    {
        parent[x] = x;
        rank[x] = 0;
    }

    public void Union(int x, int y)
    {
        int representativeX = FindSet(x);
        int representativeY = FindSet(y);

        if (rank[representativeX] == rank[representativeY])
        {
            rank[representativeY]++;
            parent[representativeX] = representativeY;
        }

        else if (rank[representativeX] > rank[representativeY])
        {
            parent[representativeY] = representativeX;
        }
        else
        {
            parent[representativeX] = representativeY;
        }
    }

    public int FindSet(int x)
    {
        if (parent[x] != x)
            parent[x] = FindSet(parent[x]);
        return parent[x];
    }

    public int FindImmidiateParent(int x)
    {
        return parent[x];
    }

    public int FindRank(int x)
    {
        return rank[x];
    }
}