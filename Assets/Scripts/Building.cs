using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [SerializeField] Button MoveButton;
    [SerializeField] Button OKButton;
    [SerializeField] Button RotateButton;

    [SerializeField] Color PreColor;
    [SerializeField] Color AfterColor;

    [SerializeField] Transform Rotatable;

    protected int xLimit, yLimit;

    public Image loadingImage;
    public TextMeshProUGUI instantiatedUnitNumberText;
    public int instantiatedUnitNumber;

    public static Action<int> BuilingApprovedAction;

    [SerializeField] SpriteRenderer[] spriteRenderers;


    public BuildingData myData;
    public BuildingSO buildingType;


    protected List<UnitData> unitDatas = new List<UnitData>();

    public List<Tile> coveredTile = new();

    bool isRotating = false;
    protected bool approved = false;
    protected float timer;

    protected virtual void Awake()
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

    protected virtual void OnEnable()
    {
        //AddEventToButton(MoveButton);
        //TODO: remove listener ondisable
        OKButton.onClick.AddListener(BuildingApproved);
        RotateButton.onClick.AddListener(RotationClicked);
    }
    
    private void Start()
    {
        ChangeColor(PreColor);
        GetTilesAtAllPositionsOfBuildings();
        foreach (var item in spriteRenderers)
        {
            item.DOFade(0.5f, 0.5f).SetLoops(-1,LoopType.Yoyo);
        }

    }

    protected void ChangeColor(Color color)
    {
        foreach (var sprite in spriteRenderers)
        {
            sprite.color = color;
        }
    }

    void SetTilesAtAllPositionsOfBuildings()
    {      
        foreach (var tile in coveredTile)
        {
            tile.hasBuilding = true;
        }
    }

    void GetTilesAtAllPositionsOfBuildings()
    {
        coveredTile.Clear();
        foreach (var sprite in spriteRenderers)
        {
            Tile tile = GridManager.Instance.GetTileAtPosition(sprite.transform.position);
            coveredTile.Add(tile);
        }
    }



    public void RotationClicked()
    {
        if (isRotating)
        {
            return;
        }
        GetTilesAtAllPositionsOfBuildings();
        isRotating = true;
        Rotatable.DORotate(new Vector3(0, 0, -90), 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutElastic).OnComplete(() => isRotating = false);
    }

    public void BuildingApproved()
    {
        //control 

        DOTween.KillAll();
        foreach (var item in spriteRenderers)
        {
            item.DOFade(1f, 1f);
        }
        SetTilesAtAllPositionsOfBuildings();
        MoveButton.gameObject.SetActive(false);
        OKButton.gameObject.SetActive(false);
        RotateButton.gameObject.SetActive(false);
        BuilingApprovedAction?.Invoke(myData.cost);
        approved = true;
    }

    protected virtual void Update()
    {
           
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

    private void OnMouseUp()
    {
        
    }

    public void OnMouseDrag()
    {
        if (approved)
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(mousePos);
       

        if (newPosition.x > xLimit)
        {
            newPosition.x = xLimit;
        }
        else if (newPosition.x < 0)
        {
            newPosition.x = 0;
        }
        if (newPosition.y > yLimit)
        {
            newPosition.y = yLimit;
        }
        else if (newPosition.y < 0)
        {
            newPosition.y = 0;
        }

        Vector3 targetPos = new Vector3(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), -1);
        transform.DOMove(targetPos, 0.2f).SetEase(Ease.Flash).OnComplete(GetTilesAtAllPositionsOfBuildings);


    }


    #endregion

}
