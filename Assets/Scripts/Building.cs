using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//TODO: inheritance for all building types.
public class Building : MonoBehaviour
{
    [SerializeField] Button MoveButton;
    [SerializeField] Button OKButton;
    [SerializeField] Button RotateButton;

    [SerializeField] Color PreColor;
    [SerializeField] Color AfterColor;

    [SerializeField] Transform Rotatable;

    public Image loadingImage;
    public TextMeshProUGUI instantiatedUnitNumberText;
    public int instantiatedUnitNumber;

    public static Action<int> BuilingApprovedAction;

    protected bool approved = false;

    [SerializeField] SpriteRenderer[] spriteRenderers;

    public BuildingData myData;

    public BuildingSO buildingType;

    protected List<UnitData> unitDatas = new List<UnitData>();


    protected float timer;

    protected void ChangeColor(Color color)
    {
        foreach (var sprite in spriteRenderers)
        {
            sprite.color = color;
        }
    }

    private void OnEnable()
    {
        //AddEventToButton(MoveButton);
        OKButton.onClick.AddListener(BuildingApproved);
        RotateButton.onClick.AddListener(RotationClicked);
    }

    private void Awake()
    {
        foreach (var item in GameData.BuildingDatas)
        {
            if (item.type == buildingType.type)
            {
                buildingType.cost = item.cost;
                buildingType.SOname = item.SOname;
                if (buildingType.unit== null)
                {
                    return;
                }
                buildingType.unit.SOname = item.unitDatas[0].SOname;
                buildingType.unit.AttackPoint = item.unitDatas[0].AttackPoint;
                buildingType.unit.DefencePower = item.unitDatas[0].DefencePower;
                buildingType.unit.Health = item.unitDatas[0].Health;
                buildingType.unit.speed = item.unitDatas[0].speed;
            }
        }
    }

    private void RotationClicked()
    {
        Rotatable.DORotate(new Vector3(0, 0, -90), 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutElastic);
    }

    private void Start()
    {
        ChangeColor(PreColor);
    }

    private void BuildingApproved()
    {
        GridManager.Instance.SetTileAfterBuilding(transform.position, buildingType.type);

        MoveButton.gameObject.SetActive(false);
        OKButton.gameObject.SetActive(false);
        RotateButton.gameObject.SetActive(false);
        BuilingApprovedAction?.Invoke(myData.cost);
        approved = true;
    }

    protected virtual void Update()
    {
        if (!approved)
        {
            return;
        }
    }

    protected virtual void CreateUnit()
    {      
        instantiatedUnitNumber++;
        instantiatedUnitNumberText.text = "x" + instantiatedUnitNumber.ToString();
    }

    public virtual void RemoveUnit()
    {
        instantiatedUnitNumber--;
        instantiatedUnitNumberText.text = "x" + instantiatedUnitNumber.ToString();
    }

    #region DRAG

    

    public void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        //mousePos.z = -Camera.main.transform.position.z;  // Adjust the Z coordinate based on the camera's distance
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 targetPos = new Vector3(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), -1);
        transform.DOMove(targetPos, 0.2f).SetEase(Ease.Flash);
    }


    #endregion

}
