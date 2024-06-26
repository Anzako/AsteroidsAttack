using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Increase your health"; }
    }

    private int level = 0;
    private int maxLevel = 3;
    private int healthValue = 2;

    public override void UpgradeFunction()
    {
        LevelManager.Instance.GetPlayerController().UpgradeHealth(healthValue);
        level += 1;

        if (level >= maxLevel) exhausted = true;

        OnUpgradeChoice?.Invoke(this);
    }
}
