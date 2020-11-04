using CodeMonkey.Utils;
using System;
using UnityEngine;

//Erstellt ein x mal x Grid welches an jeder Gridstelle ein GridObject speichert

//!!! Grid arbeitet mit normalen koordinaten isometric coorinaten müssen den funktionen umgewandelt übergeben und danach zurückgefandelt werden !!!
public class MyGrid<MyGridObject>
{
    //definition eines events und dessen argumente und argumentatribute (x und y)
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int height, width;          //anzahl der gridkästchen
    private MyGridObject[,] gridArray;  //Grid welches die objekte beinhaltet
    private float cellSize;             //größe eines Kästchens
    private Vector3 origin;             //0 punkt des grids
    private TextMesh[,] debugTextArray; //Array beinhaltet alle objekte in textform --debug
    public bool debug = true;           //aktiviert debug ausgabe

    public MyGrid(int width, int height, float cellSize, Vector3 origin, Func<MyGrid<MyGridObject>, int, int, MyGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new MyGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) //initialisierung aller gridobjekte
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this,x,y);
            }
        }

        //grid wird gezeichnet und beschriftet --debug
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

            //triggert mit dem event eine Änderung des texts im debugarray falls sich ein gridobject geändert hat
            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y].ToString();
            };
        }
    }

    //gibt die position eines Gridkästchens als coordinaten zurück
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + origin;
    }

    //gibt das passende Gridkästchen zu einer coordinate zurück
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).y / cellSize);
    }

    //triggert das event welches die debug anzeige des kästchens updatet
    public void TriggerObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    //setzt das Gridobject an der grid position gleich dem gegebenen gridobjekt
    public void SetGridObject(int x, int y, MyGridObject value)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
            //triggert event zum debug update
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    //setzt das Gridobject an der gegebenen position gleich dem gegebenen gridobjekt
    public void SetGridObject(Vector3 worldPosition, MyGridObject value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    //gibt das Gridobject in einem gewissen kästchen zurück
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

    //gibt das Gridobject an einer gewissen coordinate zurück 
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
