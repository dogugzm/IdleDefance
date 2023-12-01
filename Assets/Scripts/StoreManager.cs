using System;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public int currentMoney;

    public static Action<int> UpdateMoneyText;
    public static Action<int> ChecMoneyToBuy;

    public static StoreManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        currentMoney = 500;
    }

    private void OnEnable()
    {
        Building.BuilingApprovedAction += BuyBuilding;
        SingleBuilding.AddMoneyOnEveryXSecond += GainMoney;
    }

    private void OnDisable()
    {
        Building.BuilingApprovedAction -= BuyBuilding;
        SingleBuilding.AddMoneyOnEveryXSecond -= GainMoney;
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
