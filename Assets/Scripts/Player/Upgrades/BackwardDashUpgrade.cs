using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardDashUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Move backward and press space to dash"; }
    }

    public override void UpgradeFunction()
    {
        GameManager.GetPlayerController().GetComponent<PlayerMovement>().UnlockBackwardDash();

        OnUpgradeChoice?.Invoke(this);
    }
}

