using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public int currentMoney;

    public static Action<int> UpdateMoneyText;

    private void Awake()
    {
        currentMoney = 500;
    }

    private void OnEnable()
    {
        Building.BuilingApprovedAction += BuyBuilding;
        Building.AddMoneyOnEveryXSecond += GainMoney;

    }

    public void BuyBuilding(int cost)
    {
        currentMoney -= cost;
        UpdateMoneyText.Invoke(currentMoney);
    }

    public void GainMoney()
    {
        currentMoney += 10;
        UpdateMoneyText.Invoke(currentMoney);

    }



}