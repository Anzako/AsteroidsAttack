using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    private List<UpgradeEntity> upgrades;
    private UpgradeEntity bossUpgrade1;
    private UpgradeEntity bossUpgrade2;
    public static bool upgradesExhausted = false;

    private void Awake()
    {
        GameManager.OnStateChanged += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= GameManagerOnStateChanged;
    }

    public void GameManagerOnStateChanged(GameState state)
    {
        if (state == GameState.StartGame)
        {
            GenerateUpgrades();
        }

        if (state == GameState.UpgradeMenu)
        {
            if (LevelManager.Instance.isBossRound())
            {
                SetBossUpgradeButton(LevelManager.Instance.actualBoss);
            } 
            else
            {
                SetUpgradeButtons();
            }
        }
    }

    private void GenerateUpgrades()
    {
        upgrades = new List<UpgradeEntity>();

        /*UpgradeEntity upgrade1 = new PlayerSpeedUpgrade();
        upgrade1.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade1);

        UpgradeEntity upgrade2 = new HealthUpgrade();
        upgrade2.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade2);

        UpgradeEntity upgrade3 = new BaseWeaponUpgrade();
        upgrade3.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade3);

        UpgradeEntity upgrade4 = new ShootingSpeedUpgrade();
        upgrade4.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade4);*/

        UpgradeEntity upgrade5 = new LaserUpgrade();
        upgrade5.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade5);

        /*UpgradeEntity upgrade6 = new RocketUpgrade();
        upgrade6.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade6);*/

        bossUpgrade1 = new ForwardDashUpgrade();
        bossUpgrade1.OnUpgradeChoice += HandleBossUpgradeChoice;

        bossUpgrade2 = new BackwardDashUpgrade();
        bossUpgrade2.OnUpgradeChoice += HandleBossUpgradeChoice;
        
    }

    private void SetBossUpgradeButton(int bossLevel)
    {
        UI_ChoiceMenu.Instance.DisableButton1();
        UI_ChoiceMenu.Instance.DisableButton2();
        UI_ChoiceMenu.Instance.DisableButton3();
        UI_ChoiceMenu.Instance.EnableBossUpgradeButton();

        if (bossLevel == 1)
        {
            UI_ChoiceMenu.Instance.SetUpgradeButton(bossUpgrade1);
        } else
        {
            UI_ChoiceMenu.Instance.SetUpgradeButton(bossUpgrade2);
        }
       
    }

    private void SetUpgradeButtons()
    {
        UI_ChoiceMenu.Instance.DisableBossUpgradeButton();
        UI_ChoiceMenu.Instance.EnableButton1();
        UI_ChoiceMenu.Instance.EnableButton2();
        UI_ChoiceMenu.Instance.EnableButton3();

        if (upgrades.Count > 3)
        {
            List<UpgradeEntity> randomUpgrades = GetRandomUpgrades();
            UI_ChoiceMenu.Instance.SetButton1(randomUpgrades[0]);
            UI_ChoiceMenu.Instance.SetButton2(randomUpgrades[1]);
            UI_ChoiceMenu.Instance.SetButton3(randomUpgrades[2]);
        } else if (upgrades.Count == 3)
        {
            UI_ChoiceMenu.Instance.SetButton1(upgrades[0]);
            UI_ChoiceMenu.Instance.SetButton2(upgrades[1]);
            UI_ChoiceMenu.Instance.SetButton3(upgrades[2]);
        } else if (upgrades.Count == 2)
        {
            UI_ChoiceMenu.Instance.SetButton1(upgrades[0]);
            UI_ChoiceMenu.Instance.SetButton2(upgrades[1]);
            UI_ChoiceMenu.Instance.DisableButton3();
        } else if (upgrades.Count == 1)
        {
            UI_ChoiceMenu.Instance.SetButton2(upgrades[0]);
            UI_ChoiceMenu.Instance.DisableButton1();
        } else
        {
            UI_ChoiceMenu.Instance.DisableButton2();
        }
    }

    private List<UpgradeEntity> GetRandomUpgrades()
    {
        int count = 3;
        List<UpgradeEntity> shuffledUpgrades = new List<UpgradeEntity>(upgrades);
        System.Random rng = new System.Random();
        int n = shuffledUpgrades.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            UpgradeEntity value = shuffledUpgrades[k];
            shuffledUpgrades[k] = shuffledUpgrades[n];
            shuffledUpgrades[n] = value;
        }

        return shuffledUpgrades.GetRange(0, count);
    }

    private void HandleUpgradeChoice(UpgradeEntity upgrade)
    {
        if (upgrade.exhausted)
        {
            upgrades.Remove(upgrade);
        }

        if (upgrades.Count == 0)
        {
            upgradesExhausted = true;
        }
        GameManager.Instance.ChangeState(GameState.Game);
        LevelManager.Instance.OnNewRound();
    }

    private void HandleBossUpgradeChoice(UpgradeEntity upgrade)
    {
        GameManager.Instance.ChangeState(GameState.Game);
        LevelManager.Instance.OnNewRound();
    }
}

public abstract class UpgradeEntity
{
    public bool exhausted;
    public abstract string upgradeText { get; }
    public Action<UpgradeEntity> OnUpgradeChoice;

    public abstract void UpgradeFunction();
}
