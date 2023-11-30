using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Units", menuName = "ScriptableObjects/NewUnit", order = 1)]
public class UnitSO : ScriptableObject
{
    public string SOname;
    public int AttackPoint;
    public int Health;
    public int DefencePower;
    public float speed;
    public GameObject prefab;


   

}
