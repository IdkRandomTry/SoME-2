using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGrid : MonoBehaviour
{

    public int unit, height, width;
    private LineRenderer unitsquare;
    private Transform[] points;
    private int x, y;
    // Start is called before the first frame update
    
    private void Awake()
    {
        unitsquare = GetComponent<LineRenderer>();
    }

    public void Start()
    {
        GenerateGrid();

    }

    public void GenerateGrid()
    {
        

        for (x = 0; x < width; x=x+unit)
        {
            for (y = 0; y<height; y=y+unit)
            {
                var spawnedTile = Instantiate(unitsquare, new Vector3(x,y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
            }
        }

    }
}
