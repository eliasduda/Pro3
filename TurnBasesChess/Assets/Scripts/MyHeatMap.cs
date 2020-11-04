using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHeatMap : MonoBehaviour
{
    private MyGrid<HeatMapObject> grid;
    private Mesh mesh;

    public void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

    }
    public void SetGrid(MyGrid<HeatMapObject> grid)
    {
        grid.debug = false;
        this.grid = grid;
        UpdateHeatMap();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, MyGrid<HeatMapObject>.OnGridValueChangedEventArgs e)
    {
        Debug.Log("fired");
        UpdateHeatMap();
    }

    public void UpdateHeatMap()
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index = x * grid.GetHeight() + y;

                Vector3 quadsize = new Vector3(1, 1) * grid.GetCellsize();

                HeatMapObject gridObject = grid.GetGridObject(x, y);
                float gridValueNormalized = gridObject.GetNormalizedValue();
                Vector2 gridValueUV = new Vector2(gridValueNormalized, 0f);

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadsize * 0.5f, 0f, quadsize, gridValueUV, gridValueUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
