
using System;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject tilePreafab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private Camera cam;

    Unit clickedUnit = null;

    public static Action<int, int> TileClickEvent;
    public static Action<Unit> UnitClickEvent;
    public static Action RestartGame;


    public static GridManager Instance;

    public Dictionary<Vector2, Tile> tiles;
    List<Tile> instantiatedTileList = new();
    List<Enemy> instantiatedEnemyList = new();







    private void OnEnable()
    {
        TileClickEvent += OnTileClicked;

        UnitClickEvent += OnUnitClicked;

        RestartGame += StartGame;
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

    private void OnUnitClicked(Unit unit)
    {
        clickedUnit = unit;



    }

    private void OnTileClicked(int arg1, int arg2)
    {
        if (!clickedUnit)
        {
            return;
        }

        //TODO: if enemy on that tile ? fight : move.
        clickedUnit.MoveTowardsTile(new Vector2(arg1, arg2));

    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        foreach (var item in instantiatedTileList)
        {
            Destroy(item?.gameObject);
        }
        foreach (var item in instantiatedEnemyList)
        {
            Destroy(item?.gameObject);
        }

        GenerateGrid(GameData.GridData.width, GameData.GridData.height);


    }

    void GenerateGrid(int width, int height)
    {
        tiles = new Dictionary<Vector2, Tile>();
        instantiatedTileList.Clear();
        instantiatedEnemyList.Clear();

        int random_j = UnityEngine.Random.Range(0, height);
        int random_i = UnityEngine.Random.Range(0, width / 2);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var generatedTile = Instantiate(tilePreafab, new Vector2(i, j), Quaternion.identity);
                generatedTile.name = $"Tile {i}{j}";
                generatedTile.transform.parent = transform;
                instantiatedTileList.Add(generatedTile.GetComponent<Tile>());

                if (i < width / 2)
                {
                    generatedTile.GetComponent<Tile>().isEnemySide = true;
                }

                if (i == random_i && j == random_j)
                {
                    Debug.Log(new Vector2(i, j));
                    var generatedEnemy = Instantiate(EnemyPrefab, new Vector2(i, j), Quaternion.identity);
                    instantiatedEnemyList.Add(generatedEnemy.GetComponent<Enemy>());
                }

                bool isOffset = (i % 2 == 0 && j % 2 == 1) || (i % 2 == 1 && j % 2 == 0);

                generatedTile.GetComponent<Tile>().Initialize(isOffset, i, j);

                tiles[new Vector2(i, j)] = generatedTile.GetComponent<Tile>();

            }
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

        cam.orthographicSize = Mathf.Max(width, height) / 2 + 1;
    }


    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }

    public void SetTileAfterBuilding(Vector2 pos, BuildingTypes type)
    {
        Tile tile = GetTileAtPosition(pos);

        switch (type)
        {
            case BuildingTypes.SINGLE:
                tile.hasBuilding = true;
                break;
            case BuildingTypes.MULTIPLE:
                List<Tile> tiles = new List<Tile>();
                tiles.Add(tile);
                tiles.Add(GetTileAtPosition(new Vector2(pos.x + 1, pos.y)));
                tiles.Add(GetTileAtPosition(new Vector2(pos.x, pos.y + 1)));
                tiles.Add(GetTileAtPosition(new Vector2(pos.x + 1, pos.y + 1)));
                foreach (var item in tiles)
                {
                    item.hasBuilding = true;
                }
                break;
            case BuildingTypes.L:
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
