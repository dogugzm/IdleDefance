using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBuilding : Building
{
    //[SerializeField] UnitSO[] units;
    List<Unit> instantiatedUnits = new();

    protected override void OnEnable()
    {
        base.OnEnable();
        GridManager.RestartGame += ClearUnits;
    }

    protected override void Awake()
    {
        base.Awake();
        xLimit = GameData.GridData.width - 2;
        yLimit = GameData.GridData.height - 2;
    }

    protected override void Update()
    {
        base.Update();

        if (!approved)
        {
            return;
        }

        loadingImage.fillAmount = timer / 10;
        timer += Time.deltaTime;
        if (timer > 10)
        {
            timer = 0;
            CreateUnit();
            loadingImage.fillAmount = 0;
        }
    }

    void ClearUnits()
    {
        foreach (Unit unit in instantiatedUnits)
        {
            Destroy(unit.gameObject);
        }
    }

    protected override void CreateUnit()
    {
        base.CreateUnit();     
        GameObject instantiatedUnit = Instantiate(Resources.Load<GameObject>(buildingType.unit.SOname), transform.position, Quaternion.identity);     
        instantiatedUnit.GetComponent<Unit>().building = this;
        instantiatedUnits.Add(instantiatedUnit.GetComponent<Unit>());
    }

}
