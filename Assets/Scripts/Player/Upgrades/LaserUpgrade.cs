using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Laser damage and range increase"; }
    }

    public override void UpgradeFunction()
    {
        GameManager.GetPlayerController().GetComponent<WeaponController>().UpgradeLaser();
        exhausted = true;

        OnUpgradeChoice?.Invoke(this);
    }
}
