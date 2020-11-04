using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using Unity.Mathematics;

public class MyTesting : MonoBehaviour
{
    [SerializeField] private MyPlayer player;
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private MyHeatMap heatMap;
    private MyGrid<HeatMapObject> testGrid;
    


    private void Start()
    {
        pathfinding = new Pathfinding(5, 5);
        pathfinding.setObstacles();

        //heatmap
        //testGrid = new MyGrid<HeatMapObject>(10, 10, 10f, transform.position, (MyGrid<HeatMapObject> g, int x, int y)=>new HeatMapObject(g,x,y));
        //heatMap.SetGrid(testGrid);

    }

    private void Update()
    {
       player.handleMovement();

       if (Input.GetMouseButtonDown(0))
        {   
            /* heatmap
            Vector3 mouseP = UtilsClass.GetMouseWorldPosition();
            HeatMapObject currentValue = testGrid.GetGridObject(mouseP);
            if (currentValue != null)
            {
                currentValue.AddValue(5);
            }
            */
        }
       
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseP = UtilsClass.GetMouseWorldPosition();
            player.setTarget(mouseP);

            //DEBUG
            List<Vector3> path = pathfinding.FindPathV3(IsoMatrix.InvIso(player.transform.position), IsoMatrix.InvIso(mouseP));

            if (path != null)
            {
                for (int i = 0; i<path.Count -1; i++)
                {
                    Debug.DrawLine(path[i],  path[i+1], Color.red, 500f);

                }
            }
            
        }
    }
}

public class HeatMapObject
{
    private int min = 0, max = 100;
    private int x, y;
    private int value;
    private MyGrid<HeatMapObject> grid;
    
    public HeatMapObject(MyGrid<HeatMapObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        value = 0;
    }

    public void AddValue(int value)
    {
        this.value += value;
        Mathf.Clamp(value, min, max);
        grid.TriggerObjectChanged(x,y);
    }

    public int GetMax()
    {
        return max;
    }

    public int GetValue()
    {
        return value;
    }

    public float GetNormalizedValue()
    {
        return (float)value/max;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
