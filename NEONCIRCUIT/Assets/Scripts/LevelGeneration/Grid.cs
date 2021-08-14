using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class Grid
{
    private Tile[,] entries;
    private int width;
    private int height;

    public Tile this[int i, int j]
    {
        get
        {
            return entries[i, j];
        }
    }

    public int Width => width;
    public int Height => height;

    public Tile Start => entries[0, 0];
    public Tile End => entries[Width - 1, Height - 1];
    public int Seed { get; set; }
    private Random random;

    public void RemoveSomeWalls()
    {
        for(int i = 1; i < Width - 1; i++)
        {
            for(int j = 1; j < Height - 1; j++)
            {
                int rand = random.Next(4);
                for(int k = 0; k <= rand; k++)
                {
                    int direction = random.Next(4);
                    switch (direction)
                    {
                        case 0:
                            entries[i, j].North = true;

                            break;
                        case 1:
                            entries[i, j].South = true;
                            break;
                        case 2:
                            entries[i, j].East = true;
                            break;
                        default:
                            entries[i, j].West = true;
                            break;
                    }
                }
            }
        }
    }

    public Grid(List<Edge> mst, int width, int height, int seed)
    {
        if(seed == 0)
        {
            Seed = Environment.TickCount;
        }
        else
        {
            Seed = seed;
        }
        random = new Random(Seed);
        this.width = width;
        this.height = height;
        entries = new Tile[width, height];
        int i = 1;
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                entries[x, y] = new Tile(false, false, false, false);
                // calculate openings for each direction
                foreach(var edge in mst)
                {
                    if(edge.Vertex1 == i)
                    {
                        switch(edge.Vertex1 - edge.Vertex2)
                        {
                            case -1:
                                entries[x, y].East = true;
                                break;
                            case 1:
                                entries[x, y].West = true;
                                break;
                            default:
                                if(edge.Vertex1 - edge.Vertex2 > 0)
                                {
                                    entries[x, y].North = true;
                                }
                                else
                                {
                                    entries[x, y].South = true;
                                }
                                break;
                        }
                    }
                    if(edge.Vertex2 == i)
                    {
                        switch(edge.Vertex1 - edge.Vertex2)
                        {
                            case -1:
                                entries[x, y].West = true;
                                break;
                            case 1:
                                entries[x, y].East = true;
                                break;
                            default:
                                if(edge.Vertex1 - edge.Vertex2 > 0)
                                {
                                    entries[x, y].South = true;
                                }
                                else
                                {
                                    entries[x, y].North = true;
                                }
                                break;
                        }
                    }
                }
                i++;
            }
        }

        // open up start and end in correct directions - change direction if you want to rotate
        Start.North = true;
        //Start.East = true;
        End.East = true;
    }
}
