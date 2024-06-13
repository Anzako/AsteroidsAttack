using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Speed upgrade"; }
    }

    private int level = 0;

    public override void UpgradeFunction()
    {
        // do sth
        Debug.Log("Increase player speed by 2");
        level += 1;

        if (level >= 3) { exhausted = true; }

        OnUpgradeChoice?.Invoke(this);
    }
}
