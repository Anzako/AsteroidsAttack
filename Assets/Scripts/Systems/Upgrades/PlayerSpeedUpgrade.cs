using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedUpgrade : UpgradeEntity
{
    public override string upgradeText
    {
        get { return "Increase your speed"; }
    }

    private int level = 0;
    private float speedAmount = 0.5f;

    public override void UpgradeFunction()
    {
        GameManager.GetPlayerController().GetComponent<PlayerMovement>().IncreaseMoveSpeed(speedAmount);
        level += 1;

        if (level >= 2) { exhausted = true; }

        OnUpgradeChoice?.Invoke(this);
    }
}
