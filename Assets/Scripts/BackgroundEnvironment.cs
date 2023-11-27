using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundEnvironment : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject tilePreafab;


    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var generatedTile = Instantiate(tilePreafab, new Vector3(i, j,0), Quaternion.identity);
                generatedTile.name = $"Tile {i}{j}";
                generatedTile.transform.parent = transform;

            }
        }

    }
}
