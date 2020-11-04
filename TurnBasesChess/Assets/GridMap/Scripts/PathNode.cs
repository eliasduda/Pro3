using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public int x, y;
    private MyGrid<PathNode> grid;
    public bool isWalkable = true;

    public int gCost, hCost, fCost;
    public PathNode cameFromNode;

    public PathNode(MyGrid<PathNode> grid, int x, int y)
    {
        this.x = x;
        this.y = y;
        this.grid = grid;
    }

    public override string ToString()
    {
        return x + "," + y;
    }


    public void CalculateFCost()
    {
        fCost = hCost + gCost;
    }
}
