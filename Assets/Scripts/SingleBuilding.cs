using System;
using UnityEngine;

public class SingleBuilding : Building
{
    public static Action AddMoneyOnEveryXSecond;

    protected override void Awake()
    {
        base.Awake();
        xLimit = GameData.GridData.width - 1;
        yLimit = GameData.GridData.height - 1;
    }

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
