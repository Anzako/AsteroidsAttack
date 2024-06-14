using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSpeedUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Increase your shooting speed"; }
    }

    private int level = 0;
    private float timeAmount = 0.1f;

    public override void UpgradeFunction()
    {
        GameManager.GetPlayerController().GetComponent<WeaponController>().IncreaseShootingSpeed(timeAmount);
        level += 1;

        if (level >= 2) { exhausted = true; }

        OnUpgradeChoice?.Invoke(this);
    }
}
