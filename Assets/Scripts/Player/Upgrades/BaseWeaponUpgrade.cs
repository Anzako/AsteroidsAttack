using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Base weapon upgrade"; }
    }

    private int level = 0;

    public override void UpgradeFunction()
    {
        GameManager.GetPlayerController().GetComponent<WeaponController>().UpgradeBasicWeapon();

        level += 1;
        if (level >= 2)
        {
            exhausted = true;
        }

        OnUpgradeChoice?.Invoke(this);
    }
}
