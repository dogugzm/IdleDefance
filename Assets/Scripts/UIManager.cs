using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] private TextMeshProUGUI popUpText;

    List<Building> instantiatedBuildings = new List<Building>();
    Building tempBuilding;

    bool isPopUpActive = false;

    public static bool isUIOverride = false;


    private void Update()
    {
        isUIOverride = EventSystem.current.IsPointerOverGameObject();
    }

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
        GridManager.UnitClickEvent += AnyUnitClicked;

    }

    private void AnyUnitClicked(Unit unit)
    {
        SetPopUpText("Click Any Tile To Move Unit");
    }

    private void OnDisable()
    {
        BuildingSingleTypeButton.onClick.RemoveListener(delegate { OnBuildingBuyClicked(Building1); });
        BuildingMultiTypeButton.onClick.RemoveListener(delegate { OnBuildingBuyClicked(Building2); });
        BuildingLTypeButton.onClick.RemoveListener(delegate { OnBuildingBuyClicked(Building3); });
        CompleteButton.onClick.RemoveListener(CompleteButtonClicked);
        RotateButton.onClick.RemoveListener(RotateButtonClicked);
        CloseButton.onClick.RemoveListener(CloseButtonClicked);
        ChangeGridButton.onClick.RemoveListener(OnChangeGridButtonClicked);

        StoreManager.UpdateMoneyText -= OnBuildingBought;
        GridManager.TileClickEvent -= AnyTileClicked;
        GridManager.UnitClickEvent -= AnyUnitClicked;

    }

    private void OnChangeGridButtonClicked()
    {
        int width, height;
        width = int.Parse(WidthInput.text);
        height = int.Parse(HeightInput.text);
        //hardcoded can get from config with events.
        StoreManager.Instance.currentMoney = 500;

        if (width > 20 || height > 20)
        {
            width = 20;
            height = 20;
        }

        DataLoader.ChangeGridSize(width, height);
        SettingsPanel.SetActive(false);
        GridManager.RestartGame?.Invoke();
        foreach (Building building in instantiatedBuildings)
        {
            Destroy(building.gameObject);
        }
        instantiatedBuildings.Clear();
        tempBuilding = null;
    }

    private void CompleteButtonClicked()
    {
        //if (tempBuilding.coveredTile.Any(item => item.hasBuilding == true))
        //{
        //    return;
        //}
        foreach (Tile tile in tempBuilding.coveredTile)
        {
            if (tile.hasBuilding == true)
            {
                return;
            }
        }
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
        if (StoreManager.Instance.currentMoney<_building.cost)
        {
            return;
        }
        tempBuilding = Instantiate(_building.buildingPrefab,new Vector3(GameData.GridData.width/2, GameData.GridData.height / 2, 0f),Quaternion.identity).GetComponent<Building>();
        instantiatedBuildings.Add(tempBuilding);
        tempBuilding.buildingType = _building;
        BuildingMovementPanel.SetActive(true);
    }

    void SetPopUpText(string text)
    {
        if (isPopUpActive)
        {
            return;
        }
        isPopUpActive = true;
        popUpText.text = text; 
        popUpText.DOFade(1, 1f).OnComplete(() =>      
        { 
            popUpText.DOFade(0, 2f).OnComplete(() => isPopUpActive = false);
            
        } );
    }

    #endregion


}
