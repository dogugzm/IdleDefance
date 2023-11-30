using System;
using System.Collections.Generic;
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

    [SerializeField] private Button CompleteButton;
    [SerializeField] private Button RotateButton;

    [SerializeField] private Button ChangeGridButton;

    #endregion

    [SerializeField] private TMP_InputField WidthInput;
    [SerializeField] private TMP_InputField HeightInput;

    [Header("Panels")]
    [SerializeField] private GameObject StorePanel;
    [SerializeField] private GameObject BuildingMovementPanel;
    [SerializeField] private GameObject SettingsPanel;


    [Header("Buildings")]
    [SerializeField] private BuildingSO Building1;
    [SerializeField] private BuildingSO Building2;
    [SerializeField] private BuildingSO Building3;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI moneyText;
    List<Building> instantiatedBuildings = new List<Building>();
    Building tempBuilding;

    private void OnEnable()
    {
        BuildingSingleTypeButton.onClick.AddListener(delegate { OnBuildingBuyClicked(Building1); });
        BuildingMultiTypeButton.onClick.AddListener(delegate { OnBuildingBuyClicked(Building2); });
        BuildingLTypeButton.onClick.AddListener(delegate { OnBuildingBuyClicked(Building3); });
        CompleteButton.onClick.AddListener(CompleteButtonClicked);
        RotateButton.onClick.AddListener(RotateButtonClicked);
        CloseButton.onClick.AddListener(CloseButtonClicked);
        ChangeGridButton.onClick.AddListener(OnChangeGridButtonClicked);

        StoreManager.UpdateMoneyText += OnBuildingBought;
        GridManager.TileClickEvent += AnyTileClicked;
    }

    private void OnChangeGridButtonClicked()
    {
        DataLoader.ChangeGridSize(int.Parse(WidthInput.text), int.Parse(HeightInput.text));
        SettingsPanel.SetActive(false);
        foreach (Building building in instantiatedBuildings)
        {
            Destroy(building.gameObject);
        }
        instantiatedBuildings.Clear();
        tempBuilding = null;
        GridManager.RestartGame?.Invoke();
    }



    private void CompleteButtonClicked()
    {
        tempBuilding.BuildingApproved();
        tempBuilding = null;
        BuildingMovementPanel.SetActive(false);
    }

    private void RotateButtonClicked()
    {
        tempBuilding.RotationClicked();
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
        if (tempBuilding!=null)
        {
            return;
        }
        tempBuilding = Instantiate(_building.buildingPrefab,new Vector3(GameData.GridData.width/2, GameData.GridData.height / 2, 0f),Quaternion.identity).GetComponent<Building>();
        instantiatedBuildings.Add(tempBuilding);
        tempBuilding.buildingType = _building;
        BuildingMovementPanel.SetActive(true);
    }

    #endregion


}
