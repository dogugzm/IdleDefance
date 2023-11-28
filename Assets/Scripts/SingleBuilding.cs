using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBuilding : Building
{

    public static Action AddMoneyOnEveryXSecond;

    protected override void Update()
    {
        base.Update();
        if (!approved)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer > 1)
        {
            AddMoneyOnEveryXSecond.Invoke();
            timer = 0;
        }
        
    }

}
