
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePreafab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Camera cam;

    public static Action<int,int> TileClickEvent;

    public static GridManager Instance;

    Dictionary<Vector2, Tile> tiles;

    private void OnEnable()
    {
        TileClickEvent += OnTileClicked;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }


    private void OnTileClicked(int arg1, int arg2)
    {
        
    }

    void Start()
    {
        GenerateGrid();      
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();    
        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                var generatedTile = Instantiate(tilePreafab, new Vector2(i,j),Quaternion.identity);
                generatedTile.name = $"Tile {i}{j}";
                generatedTile.transform.parent = transform;

                if (i < width/2)
                {
                    generatedTile.GetComponent<Tile>().isEnemySide = true;
                }

                bool isOffset = (i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0);

                generatedTile.GetComponent<Tile>().Initialize(isOffset,i,j);

                tiles[new Vector2(i, j)] = generatedTile.GetComponent<Tile>();

            } 
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void SetTileAfterBuilding(Vector2 pos, UnitTypes type)
    {
        Tile tile = GetTileAtPosition(pos);

        switch (type)
        {
            case UnitTypes.SINGLE:
                tile.hasBuilding = true;
                break;
            case UnitTypes.MULTIPLE:
                List<Tile> tiles = new List<Tile>(); 
                tiles.Add(tile);
                tiles.Add(GetTileAtPosition(new Vector2(pos.x + 1 , pos.y)));
                tiles.Add(GetTileAtPosition(new Vector2(pos.x , pos.y + 1)));
                tiles.Add(GetTileAtPosition(new Vector2(pos.x + 1, pos.y + 1)));
                foreach (var item in tiles)
                {
                    item.hasBuilding = true;
                }
                break;
            case UnitTypes.L:
                //TODO: ROTATION DATA WILL ADD
                List<Tile> tilesL = new List<Tile>();
                tilesL.Add(tile);
                tilesL.Add(GetTileAtPosition(new Vector2(pos.x + 1, pos.y)));
                tilesL.Add(GetTileAtPosition(new Vector2(pos.x, pos.y + 1)));
                tilesL.Add(GetTileAtPosition(new Vector2(pos.x, pos.y + 2)));
                foreach (var item in tilesL)
                {
                    item.hasBuilding = true;
                }
                break;
            default:
                break;
        }

    }






}
