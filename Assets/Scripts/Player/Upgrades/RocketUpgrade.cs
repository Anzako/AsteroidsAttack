using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Rocket damage and explosion range increase"; }
    }

    public override void UpgradeFunction()
    {
        GameManager.GetPlayerController().GetComponent<WeaponController>().UpgradeRocket();
        exhausted = true;

        OnUpgradeChoice?.Invoke(this);
    }
}

