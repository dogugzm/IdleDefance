using System;
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
        SingleBuilding.AddMoneyOnEveryXSecond += GainMoney;
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
