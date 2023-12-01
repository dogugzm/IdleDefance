using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBuilding : Building
{
    List<Unit> instantiatedUnits = new();

    protected override void Awake()
    {
        base.Awake();
        xLimit = GameData.GridData.width - 2;
        yLimit = GameData.GridData.height - 3;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GridManager.RestartGame += ClearUnits;
    }
    protected override void OnDisable()
    {
        GridManager.RestartGame -= ClearUnits;
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
            CreateUnit(buildingType.units[0].SOname, new Vector3(coveredTile[0].transform.position.x, coveredTile[0].transform.position.y, transform.position.z));
            CreateUnit(buildingType.units[1].SOname, new Vector3(coveredTile[1].transform.position.x, coveredTile[1].transform.position.y, transform.position.z));
            CreateUnit(buildingType.units[2].SOname, new Vector3(coveredTile[2].transform.position.x, coveredTile[2].transform.position.y, transform.position.z));

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

    protected override void CreateUnit(string name, Vector3 pos)
    {
        base.CreateUnit(name, pos);
        GameObject instantiatedUnit = Instantiate(Resources.Load<GameObject>(name), pos, Quaternion.identity);
        instantiatedUnit.GetComponent<Unit>().building = this;
        instantiatedUnits.Add(instantiatedUnit.GetComponent<Unit>());
    }

}
