using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardDashUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Move forward and press space to dash"; }
    }

    public override void UpgradeFunction()
    {
        GameManager.GetPlayerController().GetComponent<PlayerMovement>().UnlockForwardDash();

        OnUpgradeChoice?.Invoke(this);
    }
}
