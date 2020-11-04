using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Algorithmus welcher den schnellsten pfad auf einem Grid berechnet (benutzt MyGrid)
public class Pathfinding
{
    public static Pathfinding Instance { get; private set; } //Instanz variable zeigt auf das gerade aktive Pathfinding object

    private const int MOVE_DIAGONAL_COST = 14, MOVE_STRAIGHT_COST = 10; //Kosten der einzelnen bewegungen
    private MyGrid<PathNode> grid;
    private List<PathNode> openList, closedList;
    public Pathfinding(int width, int height)
    {
        Instance = this;
        grid = new MyGrid<PathNode>(width, height, 10f, Vector3.zero, (MyGrid<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }

    public List<Vector3> FindPathV3(Vector3 startWorld, Vector3 endWorld)
    {
        grid.GetXY(startWorld,  out int startx, out int starty);
        Debug.Log("Player =" + startx + "/" + starty);
        grid.GetXY(endWorld, out int endx, out int endy);
        Debug.Log("Target =" + endx + "/" + endy);
        List<PathNode> path = this.FindPath(startx, starty, endx, endy);
        if(path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorpath = new List<Vector3>();
            foreach (PathNode node in path)
            {
                vectorpath.Add(IsoMatrix.Iso(new Vector3(node.x ,node.y) * grid.GetCellsize() + Vector3.one * grid.GetCellsize() * 0.5f));
            }

            return vectorpath;
        }
        
    }
    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode>{startNode};
        closedList = new List<PathNode>();

        for(int x = 0; x < grid.GetWidth(); x++)
        { 
            for( int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if(currentNode == endNode)
            {
                //reached goal
                return CalculateFinalPath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbours(currentNode)){
                if (neighbourNode != null)
                {
                    if (closedList.Contains(neighbourNode)) { continue; }
                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }
        }
        Debug.Log("There is no path");
        // out of Nodes in openlist
        return null;
    }

    private List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbourNodes = new List<PathNode>();
        
        if (currentNode.x - 1 >= 0)
        {
            neighbourNodes.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y));
            if(currentNode.y - 1 >= 0) { neighbourNodes.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y - 1)); }
            if (currentNode.y + 1 < grid.GetHeight()) { neighbourNodes.Add(grid.GetGridObject(currentNode.x - 1, currentNode.y + 1)); }
        }
        if (currentNode.x + 1 >= 0)
        {
            neighbourNodes.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y));
            if (currentNode.y - 1 >= 0) { neighbourNodes.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y - 1)); }
            if (currentNode.y + 1 < grid.GetHeight()) { neighbourNodes.Add(grid.GetGridObject(currentNode.x + 1, currentNode.y + 1)); }
        }
        if (currentNode.y - 1 >= 0) { neighbourNodes.Add(grid.GetGridObject(currentNode.x, currentNode.y - 1)); }
        if (currentNode.y + 1 < grid.GetHeight()) { neighbourNodes.Add(grid.GetGridObject(currentNode.x, currentNode.y + 1)); }

        return neighbourNodes;
    }

    private List<PathNode> CalculateFinalPath(PathNode finalNode)
    {
        List<PathNode> path = new List<PathNode>();
        path.Add(finalNode);
        PathNode currentNode = finalNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return Mathf.Min(xDistance, yDistance) * MOVE_DIAGONAL_COST + remaining * MOVE_STRAIGHT_COST;


    }

    private PathNode GetLowestFCostNode(List<PathNode> NodeList)
    {
        PathNode lowestFCostNode = NodeList[0];

        for(int i = 0; i < NodeList.Count; i++)
        {
            if(NodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = NodeList[i];
            }
        }
        return lowestFCostNode;
    }
    
    public MyGrid<PathNode> GetGrid()
    {
        return grid;
    }

    public void setObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach(GameObject obs in obstacles)
        {
            grid.GetXY(IsoMatrix.InvIso(obs.transform.position), out int x, out int y);
            grid.GetGridObject(x, y).isWalkable = false;
        }
    }

}
