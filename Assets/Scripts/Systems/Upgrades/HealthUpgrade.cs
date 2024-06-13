using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Increase health by 2"; }
    }

    private int level = 0;
    private int healthValue = 2;

    public override void UpgradeFunction()
    {
        LevelManager.Instance.GetPlayerController().UpgradeHealth(healthValue);
        level += 1;

        if (level >= 2) exhausted = true;

        OnUpgradeChoice?.Invoke(this);
    }
}
