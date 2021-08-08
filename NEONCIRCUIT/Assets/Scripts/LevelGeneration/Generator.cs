using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private static System.Random random = new System.Random();
    
    public int gridSizeX = 10;
    public int gridSizeZ = 10;

    public float prefabSizeX;
    public float prefabSizeZ;

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


    public List<GameObject> prefabEmpty;
    public List<GameObject> prefabSingle;
    public List<GameObject> prefabDouble;
    public List<GameObject> prefabTriple;
    public List<GameObject> prefabClosed;

    void Start()
    {
        
        List<Edge> edges = new List<Edge>();
            for(int y = 0; y < gridSizeZ; y++)
            {
                for(int x = 1; x <= gridSizeX; x++)
                {
                    int current = y * gridSizeX + x;
                    if(y > 0)
                    {
                        int changed_y = (y - 1) * gridSizeX + x;
                        edges.Add(new Edge(current, changed_y, random.Next(1, gridSizeX * gridSizeZ)));
                    }
                    if(x > 1)
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
            
            PlacePrefabs(maze);
    }

    void PlacePrefabs(Grid maze)
    {
        // TODO: delete some prefabs and use rotation
        for(int x = 0; x < maze.Width; x++)
        {
            for(int z = 0; z < maze.Height; z++)
            {
                float scaledX = x * prefabSizeX;
                float scaledZ = z * prefabSizeZ;
                if(maze[x, z].North && maze[x, z].South && maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXZNegXZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                    /*int rand = random.Next(prefabEmpty.Count);
                    Instantiate(prefabEmpty[rand], new Vector3(scaledX, 0, scaledZ), Quaternion.identity);*/
                }
                else if(maze[x, z].North && maze[x, z].South && maze[x, z].East)
                {
                    Instantiate(prefabPosXZNegZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].North && maze[x, z].South && maze[x, z].West)
                {
                    //Instantiate(prefabPosZNegXZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                    GameObject temp = Instantiate(prefabPosXZNegZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                    temp.transform.RotateAround(temp.transform.position + new Vector3(prefabSizeX * 0.5f, 0f, prefabSizeZ * 0.5f), Vector3.up, 180f);
                }
                else if(maze[x, z].North && maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXNegXZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].South && maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXZNegX, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].North && maze[x, z].South)
                {
                    Instantiate(prefabPosZNegZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].North && maze[x, z].East)
                {
                    Instantiate(prefabPosXNegZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].North && maze[x, z].West)
                {
                    Instantiate(prefabNegXZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].South && maze[x, z].East)
                {
                    Instantiate(prefabPosXZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].South && maze[x, z].West)
                {
                    Instantiate(prefabPosZNegX, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].East && maze[x, z].West)
                {
                    Instantiate(prefabPosXNegX, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].North)
                {
                    Instantiate(prefabNegZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].South)
                {
                    Instantiate(prefabPosZ, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].East)
                {
                    Instantiate(prefabPosX, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
                else if(maze[x, z].West)
                {
                    Instantiate(prefabNegX, new Vector3(scaledX, 0, scaledZ), Quaternion.identity);
                }
            }
        }
    }
}
