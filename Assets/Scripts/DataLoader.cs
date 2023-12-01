using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class GridData
{
    public int width;
    public int height;
}

[System.Serializable]
public class BuildingData
{
    public string SOname;
    public int cost;
    public BuildingTypes type;
    public List<UnitData> unitDatas;
}

[System.Serializable]
public class UnitData
{
    public string SOname;
    public int AttackPoint;
    public int Health;
    public int DefencePower;
    public float speed;
}

[System.Serializable]
public class GameConfig
{
    //public GridData gridData;
    public List<BuildingData> buildingDatas;
    //public List<UnitData> unitDatas;
}

public class DataLoader : MonoBehaviour
{

    void Start()
    {
        CreateJsonFile();
        LoadJsonFile();
    }

    void LoadJsonFile()
    {

        string filePath = Path.Combine(Application.persistentDataPath, "gameConfig.json");
        string filePathGrid = Path.Combine(Application.persistentDataPath, "gameConfigGrid.json");


        string jsonBuilding = System.IO.File.ReadAllText(filePath);
        string jsonGrid = System.IO.File.ReadAllText(filePathGrid);

        GameConfig gameConfig = JsonConvert.DeserializeObject<GameConfig>(jsonBuilding);
        GridData gridConfig = JsonConvert.DeserializeObject<GridData>(jsonGrid);
        GameData.BuildingDatas = gameConfig.buildingDatas;
        GameData.GridData = gridConfig;

    }

    /// <summary>
    /// Changes and push json file
    /// </summary>
    /// <param name="_width"></param>
    /// <param name="_height"></param>
    public static void ChangeGridSize(int _width, int _height)
    {
        GameData.GridData.width = _width;
        GameData.GridData.height = _height;

        GridData gridConfig = new GridData
        {
            width = _width,
            height = _height,
        };

        string jsonGrid = JsonConvert.SerializeObject(gridConfig, Newtonsoft.Json.Formatting.Indented);
        string filePathGrid = Path.Combine(Application.persistentDataPath, "gameConfigGrid.json");
        System.IO.File.WriteAllText(filePathGrid, jsonGrid);

    }

    /// <summary>
    /// for initial writing for testing purposes
    /// </summary>
    void CreateJsonFile()
    {
        List<UnitData> unitDataListForMultiple = new List<UnitData>
        {
            new UnitData { SOname = "Yaya1", AttackPoint = 20, Health = 100, DefencePower = 10, speed = 0.3f},
            new UnitData { SOname = "Yaya2", AttackPoint = 30, Health = 120, DefencePower = 15, speed = 0.4f},
        };

        List<UnitData> unitDataListForL = new List<UnitData>
        {
            new UnitData { SOname = "Atlı1", AttackPoint = 20, Health = 100, DefencePower = 10, speed = 0.3f},
            new UnitData { SOname = "Atlı2", AttackPoint = 30, Health = 120, DefencePower = 15, speed = 0.4f},
            new UnitData { SOname = "Atlı3", AttackPoint = 30, Health = 120, DefencePower = 15, speed = 0.4f},

        };

        List<UnitData> unitDataListForSingle = new();

        List<BuildingData> buildingDataList = new List<BuildingData>
        {
            new BuildingData { SOname = "SingleBuilding", cost = 100, type = BuildingTypes.SINGLE,unitDatas = unitDataListForSingle },
            new BuildingData { SOname = "MultiBuilding", cost = 200, type = BuildingTypes.MULTIPLE, unitDatas = unitDataListForMultiple },
            new BuildingData { SOname = "LBuilding", cost = 300, type = BuildingTypes.L , unitDatas = unitDataListForL},
        };


        GameConfig gameConfig = new GameConfig
        {
            //gridData = new GridData { width = 16, height = 9},
            buildingDatas = buildingDataList,
            //unitDatas = unitDataList
        };

        GridData gridConfig = new GridData
        {
            //gridData = new GridData { width = 16, height = 9},
            width = 15,
            height = 15,
            //unitDatas = unitDataList
        };

        // Convert the object to JSON
        string json = JsonConvert.SerializeObject(gameConfig, Newtonsoft.Json.Formatting.Indented);
        string jsonGrid = JsonConvert.SerializeObject(gridConfig, Newtonsoft.Json.Formatting.Indented);


        // Specify the file path (you can adjust this path as needed)
        string filePath = Path.Combine(Application.persistentDataPath, "gameConfig.json");
        string filePathGrid = Path.Combine(Application.persistentDataPath, "gameConfigGrid.json");


        // Write the JSON to a file
        System.IO.File.WriteAllText(filePath, json);
        System.IO.File.WriteAllText(filePathGrid, jsonGrid);


        //Debug.Log("JSON file created: " + filePath);
    }


}