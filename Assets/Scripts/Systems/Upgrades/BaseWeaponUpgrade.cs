using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Base weapon upgrade"; }
    }

    private int level = 1;

    public override void UpgradeFunction()
    {
        // do sth
        Debug.Log("Base weapon upgrade");

        // upgrade level up
        level += 1;
        exhausted = true;

        OnUpgradeChoice?.Invoke(this);
    }
}
