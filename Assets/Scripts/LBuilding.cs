using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBuilding : Building
{

    protected override void Awake()
    {
        base.Awake();
        xLimit = GameData.GridData.width - 2;
        yLimit = GameData.GridData.height - 3;
    }

}
