using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Generator : MonoBehaviour
{
    private System.Random random;

    public int gridSizeX = 10;
    public int gridSizeZ = 10;

    public float prefabSizeX;
    public float prefabSizeZ;

    public Vector3 endPoint = new Vector3(0f, 0f, 0f);

    // prefabs for all opening directions
    public GameObject prefabPosX;
    public GameObject prefabPosZ;
    public GameObject prefabPosXZ;
    public GameObject prefabNegX;
    public GameObject prefabNegZ;
    public GameObject prefabNegXZ;
    public GameObject prefabPosXNegZ;
    public GameObject prefabPosZNegX;
    public GameObject prefabPosXNegX;
    public GameObject prefabPosZNegZ;
    public GameObject prefabPosXZNegX;
    public GameObject prefabPosXNegXZ;
    public GameObject prefabPosXZNegZ;
    public GameObject prefabPosZNegXZ;
    public GameObject prefabPosXZNegXZ;

    public GameObject prefabHealthPickup;
    public int healthPickupCount = 3;

    public List<GameObject> prefabEmpty;
    public List<GameObject> prefabSingle;
    public List<GameObject> prefabDouble;
    public List<GameObject> prefabTriple;
    public List<GameObject> prefabClosed;

    public List<GameObject> enemies;

    private int seed;
    private int gridSeed;

    public void Save(string path)
    {
        using (StreamWriter wr = new StreamWriter(path, false))
        {
            // add other stuff
            wr.WriteLine(seed);
            wr.WriteLine(gridSeed);
        }
    }

    void Start()
    {
        seed = Environment.TickCount;
        random = new System.Random(2574796);
        Debug.Log("SEED: " + seed);
        List<Edge> edges = new List<Edge>();
        for (int y = 0; y < gridSizeZ; y++)
        {
            for (int x = 1; x <= gridSizeX; x++)
            {
                int current = y * gridSizeX + x;
                if (y > 0)
                {
                    int changed_y = (y - 1) * gridSizeX + x;
                    edges.Add(new Edge(current, changed_y, random.Next(1, gridSizeX * gridSizeZ)));
                }
                if (x > 1)
                {
                    int changed_x = y * gridSizeX + x - 1;
                    edges.Add(new Edge(current, changed_x, random.Next(1, gridSizeX * gridSizeZ)));
                }
            }
        }

        List<int> vertices = new List<int>();
        for (int i = 1; i <= gridSizeX * gridSizeZ; i++)
        {
            vertices.Add(i);
        }

        List<Edge> mst = Kruskal.GetMinimumSpanningTree(edges, vertices);
        Grid maze = new Grid(mst, gridSizeX, gridSizeZ);
        maze.RemoveSomeWalls();

        Debug.Log("Grid: " + maze.Seed);

        // random health pickup tiles
        List<Tuple<int, int>> containedIndices = new List<Tuple<int, int>>();
        int j = 0;
        while (j < healthPickupCount)
        {
            int x = random.Next(maze.Width - 2);
            int y = random.Next(maze.Height - 2);
            Tuple<int, int> temp = new Tuple<int, int>(x, y);
            for (int k = 0; k < containedIndices.Count; k++)
            {
                if (containedIndices[k].Item1 != x || containedIndices[k].Item2 != y)
                {
                    containedIndices.Add(temp);
                    maze[x, y].HasHealthPickup = true;
                    j++;
                    break;
                }
            }
            if (containedIndices.Count == 0)
            {
                containedIndices.Add(temp);
                maze[x, y].HasHealthPickup = true;
                j++;
            }
        }
        PlacePrefabs(maze);
    }

    void PlacePrefabs(Grid maze)
    {
        // calculate startpoint
        float width = prefabSizeX * gridSizeX;
        float depth = prefabSizeZ * gridSizeZ;
        Vector3 startpoint = new Vector3(endPoint.x - width, endPoint.y, endPoint.z - depth);

        // TODO: delete some prefabs and use rotation
        for (int x = 0; x < maze.Width; x++)
        {
            for (int z = 0; z < maze.Height; z++)
            {
                float scaledX = x * prefabSizeX;
                float scaledZ = z * prefabSizeZ;
                Vector3 placePos = startpoint + new Vector3(scaledX, endPoint.y + 1.0f, scaledZ);

                // PICKUPS

                if (maze[x, z].HasHealthPickup)
                {
                    //Instantiate(prefabHealthPickup, placePos + new Vector3(0.5f * prefabSizeX, 0f, 0.5f * prefabSizeZ), Quaternion.identity);

                    Instantiate(enemies[0], placePos, Quaternion.identity);
                }

                // TERRAIN

                if (maze[x, z].North && maze[x, z].South && maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXZNegXZ, placePos, Quaternion.identity);
                    /*int rand = random.Next(prefabEmpty.Count);
                    Instantiate(prefabEmpty[rand], new Vector3(scaledX, 0, scaledZ), Quaternion.identity);*/
                }
                else if (maze[x, z].North && maze[x, z].South && maze[x, z].East)
                {
                    Instantiate(prefabPosXZNegZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].North && maze[x, z].South && maze[x, z].West)
                {
                    Instantiate(prefabPosZNegXZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].North && maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXNegXZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].South && maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXZNegX, placePos, Quaternion.identity);
                }
                else if (maze[x, z].North && maze[x, z].South)
                {
                    Instantiate(prefabPosZNegZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].North && maze[x, z].East)
                {
                    Instantiate(prefabPosXNegZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].North && maze[x, z].West)
                {
                    Instantiate(prefabNegXZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].South && maze[x, z].East)
                {
                    Instantiate(prefabPosXZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].South && maze[x, z].West)
                {
                    Instantiate(prefabPosZNegX, placePos, Quaternion.identity);
                }
                else if (maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXNegX, placePos, Quaternion.identity);
                }
                else if (maze[x, z].North)
                {
                    Instantiate(prefabNegZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].South)
                {
                    Instantiate(prefabPosZ, placePos, Quaternion.identity);
                }
                else if (maze[x, z].East)
                {
                    Instantiate(prefabPosX, placePos, Quaternion.identity);
                }
                else if (maze[x, z].West)
                {
                    Instantiate(prefabNegX, placePos, Quaternion.identity);
                }
            }
        }
    }
}
