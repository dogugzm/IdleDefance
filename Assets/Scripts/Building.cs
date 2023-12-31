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
                if (item.unitDatas.Count <= 0)
                {
                    return;
                }

                for (int i = 0; i < item.unitDatas.Count; i++)
                {
                    buildingType.units[i].SOname = item.unitDatas[i].SOname;
                    buildingType.units[i].AttackPoint = item.unitDatas[i].AttackPoint;
                    buildingType.units[i].DefencePower = item.unitDatas[i].DefencePower;
                    buildingType.units[i].Health = item.unitDatas[i].Health;
                    buildingType.units[i].speed = item.unitDatas[i].speed;
                }          
            }
        }

    }

    protected virtual void OnEnable()
    {
        
        OKButton.onClick.AddListener(BuildingApproved);
        RotateButton.onClick.AddListener(RotationClicked);
    }

    protected virtual void OnDisable()
    {
        OKButton.onClick.RemoveListener(BuildingApproved);
        RotateButton.onClick.RemoveListener(RotationClicked);
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
            Tile tile = GridManager.Instance.GetTileAtPosition(new Vector2(Mathf.RoundToInt( sprite.transform.position.x), Mathf.RoundToInt(sprite.transform.position.y)));
            coveredTile.Add(tile);
        }
    }

    public void RotationClicked()
    {
        if (isRotating)
        {
            return;
        }
        isRotating = true;
        Rotatable.DORotate(new Vector3(0, 0, -90), 0.5f, RotateMode.WorldAxisAdd).SetEase(Ease.OutElastic).OnComplete(RotatinComplete);
    }

    void RotatinComplete()
    {
        isRotating = false;
        GetTilesAtAllPositionsOfBuildings();
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
        BuilingApprovedAction?.Invoke(buildingType.cost);
        approved = true;
    }

    protected virtual void Update()
    {
           
    }

    protected virtual void CreateUnit(string name, Vector3 pos)
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

        Vector3 targetPos = new Vector3(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y), 0);
        transform.DOMove(targetPos, 0.2f).SetEase(Ease.Flash).OnComplete(GetTilesAtAllPositionsOfBuildings);
    }


    #endregion

}
