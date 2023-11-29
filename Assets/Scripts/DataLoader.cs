using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
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
    public GridData gridData;
    public List<BuildingData> buildingDatas;
    //public List<UnitData> unitDatas;
}

public class DataLoader : MonoBehaviour
{
    public string configFilePath = "Assets/config.json";

    void Start()
    {
        CreateJsonFile();

        LoadJsonFile();
    }

    void LoadJsonFile()
    {
        string filePath = Application.dataPath + "/gameConfig.json";

        string json = System.IO.File.ReadAllText(filePath);

        GameConfig gameConfig = JsonConvert.DeserializeObject<GameConfig>(json);

        GameData.GridData = gameConfig.gridData;
        //GameData.unitDatas = gameConfig.unitDatas;
        GameData.BuildingDatas = gameConfig.buildingDatas;

        //Debug.Log("Grid Width: " + gameConfig.gridData.width);
        //Debug.Log("Building Count: " + gameConfig.buildingDatas.Count);
        //Debug.Log("Unit Count: " + gameConfig.unitDatas.Count);

        //if (gameConfig.buildingDatas.Count > 0)
        //{
        //    BuildingData firstBuilding = gameConfig.buildingDatas[0];
        //    Debug.Log("First Building Name: " + firstBuilding.SOname);
        //}

        //if (gameConfig.unitDatas.Count > 0)
        //{
        //    UnitData firstUnit = gameConfig.unitDatas[0];
        //    Debug.Log("First Unit Name: " + firstUnit.SOname);
        //}
    }

    void CreateJsonFile()
    {
        List<UnitData> unitDataListForSingle = new List<UnitData>
        {
            new UnitData { SOname = "Yaya1", AttackPoint = 20, Health = 100, DefencePower = 10, speed = 0.3f},
            new UnitData { SOname = "Yaya2", AttackPoint = 30, Health = 120, DefencePower = 15, speed = 0.4f},        
        };

        List<BuildingData> buildingDataList = new List<BuildingData>
        {
            new BuildingData { SOname = "SingleBuilding", cost = 100, type = BuildingTypes.SINGLE },
            new BuildingData { SOname = "MultiBuilding", cost = 200, type = BuildingTypes.MULTIPLE, unitDatas = unitDataListForSingle },
            new BuildingData { SOname = "LBuilding", cost = 300, type = BuildingTypes.L },

        };


        GameConfig gameConfig = new GameConfig
        {
            gridData = new GridData { width = 16, height = 9},
            buildingDatas = buildingDataList,
            //unitDatas = unitDataList
        };

        // Convert the object to JSON
        string json = JsonConvert.SerializeObject(gameConfig, Newtonsoft.Json.Formatting.Indented);

        // Specify the file path (you can adjust this path as needed)
        string filePath = Application.dataPath + "/gameConfig.json";

        // Write the JSON to a file
        System.IO.File.WriteAllText(filePath, json);

        //Debug.Log("JSON file created: " + filePath);
    }


}