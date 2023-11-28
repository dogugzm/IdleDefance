using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBuilding : Building
{
    [SerializeField] UnitSO[] units;
    List<Unit> instantiatedUnits = new();

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

    protected override void CreateUnit()
    {
        base.CreateUnit();     
        var instantiatedUnit = Instantiate(units[0].prefab, transform.position, Quaternion.identity);
        instantiatedUnit.GetComponent<Unit>().building = this;
        instantiatedUnits.Add(instantiatedUnit.GetComponent<Unit>());
        //instantiatedUnit.GetComponent<Unit>().targetVector = new Vector2(0, 8);
    }

}
