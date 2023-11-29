using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingTypes
{
    SINGLE,
    MULTIPLE,
    L
}

[CreateAssetMenu(fileName = "Buildings", menuName = "ScriptableObjects/NewBuilding", order = 1)]
public class BuildingSO : ScriptableObject
{
    public string SOname;
    public int cost;
    public BuildingTypes type;
    public GameObject buildingPrefab;
    public UnitSO unit;
 
}
