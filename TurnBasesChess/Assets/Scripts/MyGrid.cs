using CodeMonkey.Utils;
using System;
using UnityEngine;

public class MyGrid<MyGridObject>
{

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int height, width;
    private MyGridObject[,] gridArray;
    private float cellSize;
    private Vector3 origin;
    private TextMesh[,] debugTextArray;
    public bool debug = true;

    public MyGrid(int width, int height, float cellSize, Vector3 origin, Func<MyGrid<MyGridObject>, int, int, MyGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new MyGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this,x,y);
            }
        }

        if (debug)
        {
            for (int i = 0; i < gridArray.GetLength(0); i++)
            {
                for (int j = 0; j < gridArray.GetLength(1); j++)
                {
                    debugTextArray[i, j] = UtilsClass.CreateWorldText(gridArray[i, j]?.ToString(), null, IsoMatrix.Iso(GetWorldPosition(i, j) + new Vector3(cellSize, cellSize) * 0.5f), 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(IsoMatrix.Iso(GetWorldPosition(i, j)), IsoMatrix.Iso(GetWorldPosition(i, j + 1)), Color.white, 100f);
                    Debug.DrawLine(IsoMatrix.Iso(GetWorldPosition(i, j)), IsoMatrix.Iso(GetWorldPosition(i + 1, j)), Color.white, 100f);
                }
            }
            Debug.DrawLine(IsoMatrix.Iso(GetWorldPosition(0, height)), IsoMatrix.Iso(GetWorldPosition(width, height)), Color.white, 100f);
            Debug.DrawLine(IsoMatrix.Iso(GetWorldPosition(width, 0)), IsoMatrix.Iso(GetWorldPosition(width, height)), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }


    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    }

    public void TriggerObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(int x, int y, MyGridObject value)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, MyGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public MyGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return gridArray[x, y];

        } else
        {
            return default(MyGridObject);
        }
    }

    public MyGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public float GetCellsize()
    {
        return cellSize;
    }
}
