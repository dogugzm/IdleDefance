using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    #region BUTTONS
    [SerializeField] private Button BuildingSingleTypeButton;
    [SerializeField] private Button BuildingMultiTypeButton;
    [SerializeField] private Button BuildingLTypeButton;
    [SerializeField] private Button CloseButton;
    #endregion

    [Header("Panels")]
    [SerializeField] private GameObject StorePanel;

    [Header("Buildings")]
    [SerializeField] private BuildingSO Building1;
    [SerializeField] private BuildingSO Building2;
    [SerializeField] private BuildingSO Building3;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI moneyText;

    private void OnEnable()
    {
        BuildingSingleTypeButton.onClick.AddListener(delegate { OnBuildingBuyClicked(Building1); });
        BuildingMultiTypeButton.onClick.AddListener(delegate { OnBuildingBuyClicked(Building2); });
        BuildingLTypeButton.onClick.AddListener(delegate { OnBuildingBuyClicked(Building3); });
        CloseButton.onClick.AddListener(CloseButtonClicked);
        StoreManager.UpdateMoneyText += OnBuildingBought;
        GridManager.TileClickEvent += AnyTileClicked;
    }

    private void OnBuildingBought(int data)
    {
        //CloseButtonClicked();
        moneyText.text = "Money:  " + data;
    }
    private void AnyTileClicked(int arg1, int arg2)
    {
        //OpenStorePanel();
    }

    private void OpenStorePanel()
    {
        StorePanel.SetActive(true);
    }

    #region ClickEvents

    private void CloseButtonClicked()
    {
        StorePanel.SetActive(false);
    }

    private void OnBuildingBuyClicked(BuildingSO _building)
    {
        Building instantiatedGO = Instantiate(_building.buildingPrefab,new Vector3(8,4,-1f),Quaternion.identity).GetComponent<Building>();
        instantiatedGO.data = _building;
    }

    #endregion


}
