using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;

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

    public GameObject prefabSpawner;

    public List<GameObject> prefabEmpty;
    public List<GameObject> prefabSingle;
    public List<GameObject> prefabDouble;
    public List<GameObject> prefabTriple;
    public List<GameObject> prefabClosed;

    public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
    public GameObject navmeshObject;

    public int Seed = 0;

    public int GridSeed = 0;

    public int healthPercentage;
    public int enemyPercentage;

    void Start()
    {
        if(Seed == 0)
        {
            Seed = Environment.TickCount;
        }
        random = new System.Random(Seed);

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
        Grid maze = new Grid(mst, gridSizeX, gridSizeZ, this);

        // make world more open
        maze.RemoveSomeWalls();

        for(int i = 1; i < maze.Width; i++)
        {
            for(int j = 1; j < maze.Height; j++)
            {
                int rand = random.Next(100);
                if(rand < healthPercentage)
                {
                    maze[i, j].HasHealthPickup = true;
                }
                else if(rand < enemyPercentage)
                {
                    maze[i, j].HasSpawner = true;
                }
            }
        }

        maze[0, 0].HasSpawner = false;
        maze[1, 0].HasSpawner = false;
        maze[0, 1].HasSpawner = false;
        maze[1, 1].HasSpawner = false;

        PlacePrefabs(maze);
        CalculateNavmeshsofGameObject();
    }

    void CalculateNavMeshSurfaces()
    {
        foreach(var surface in surfaces)
        {
            surface.BuildNavMesh();
        }
    }

    void CalculateNavmeshsofGameObject()
    {
        foreach(NavMeshSurface surface in navmeshObject.GetComponents<NavMeshSurface>())
        {
            surface.BuildNavMesh();
        }
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
                Vector3 placePos = startpoint + new Vector3(scaledX, endPoint.y, scaledZ);

                // PICKUPS

                if (maze[x, z].HasHealthPickup)
                {
                    Instantiate(prefabHealthPickup, placePos + new Vector3(0.5f * prefabSizeX, 1f, 0.5f * prefabSizeZ), Quaternion.identity);
                }
                
                // ENEMIES

                if(maze[x, z].HasSpawner)
                {
                    Instantiate(prefabSpawner, placePos + new Vector3(0.5f * prefabSizeX, 0f, 0.5f * prefabSizeZ), Quaternion.identity);
                }

                // TERRAIN

                if (maze[x, z].North && maze[x, z].South && maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXZNegXZ, placePos, Quaternion.identity);
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
