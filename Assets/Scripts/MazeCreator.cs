using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    public bool[,] visited;
    public bool[,] hwalls;
    public bool[,] vwalls;
    public int size;
    private int unvisited = 0;
    public float scale = 5;

    public bool create;

    void Start()
    {
        if(create)
            instantiateMaze();
    }

    public void instantiateMaze()
    {
        ResetWalls();
        AldousBroder();

        foreach (var maze in FindObjectsOfType<CreateMaze>())
        {
            maze.Create(size, scale, hwalls, vwalls);
        }
    }

    private void ResetWalls()
    {
        visited = new bool[size, size];
        hwalls = new bool[size + 1, size + 1];
        vwalls = new bool[size + 1, size + 1];
        fill(hwalls, true);
        fill(vwalls, true);
    }

    void AldousBroder()
    {
        int[] p = { 0, 0 };
        visit(p);

        while (unvisited < size * size)
        {
            var next = pickNeighbor(p);
            if (!isvisited(next))
            {
                visit(next);
                removeWall(p, next);
            }
            p = next;
        }
    }

    void fill(bool[,] M, bool b)
    {
        for (int i = 0; i < size + 1; i++)
        {
            for (int j = 0; j < size + 1; j++)
            {
                M[i, j] = b;
            }
        }
    }

    int[] pickNeighbor(int[] p)
    {
        var i = p[0];
        var j = p[1];
        int[,] neighbors = new int[,]
        {
            {i-1,j },
            {i+1,j },
            {i,j+1 },
            {i,j-1 }
        };

        return pickValid(neighbors);
    }

    void visit(int[] p)
    {
        visited[p[0], p[1]] = true;
        unvisited++;
        printCurr(p);
    }

    string pointString(int[] p)
    {
        return p[0].ToString() + "," + p[1].ToString();
    }

    void printCurr(int[] p)
    {
        string s = pointString(p);
        // Debug.Log("visiting: " + unvisited.ToString() + " : " + s);
    }
    bool isvisited(int[] p)
    {
        return visited[p[0], p[1]];
    }

    int[] pick(int[,] neighbors)
    {
        int n = rand(4);

        return Enumerable.Range(0, 2)
            .Select(x => neighbors[n, x])
            .ToArray();
    }

    bool checkValid(int[] point)
    {
        bool ival = 0 <= point[0] && point[0] < size;
        bool jval = 0 <= point[1] && point[1] < size;
        return ival && jval;
    }

    int[] pickValid(int[,] neighbors)
    {
        var n = pick(neighbors);
        bool b = checkValid(n);
        while (!b)
        {
            n = pick(neighbors);
            b = checkValid(n);
        }
        return n;
    }
    void removeWall(int[] p1, int[] p2)
    {
        int dx = p2[0] - p1[0];
        int dy = p2[1] - p1[1];
        if (dx == -1)
        {
            var i = p1[0];
            var j = p1[1];
            vwalls[i, j] = false;
            // Debug.Log("Removing vwall " + i.ToString() + "," + j.ToString());
        }
        if (dy == -1)
        {
            var i = p1[0];
            var j = p1[1];
            hwalls[i, j] = false;
            // Debug.Log("Removing hwall " + i.ToString() + "," + j.ToString());
        }
        if (dx == 1)
        {
            var i = p1[0] + dx;
            var j = p1[1];
            vwalls[i, j] = false;
            // Debug.Log("Removing vwall " + i.ToString() + "," + j.ToString());
        }
        if (dy == 1)
        {
            var i = p1[0];
            var j = p1[1] + dy;
            hwalls[i, j] = false;
            // Debug.Log("Removing hwall " + i.ToString() + "," + j.ToString());
        }
    }

    int rand(int size)
    {
        return (int)Random.Range(0.0f, (float)size - 0.001f);
    }
}
