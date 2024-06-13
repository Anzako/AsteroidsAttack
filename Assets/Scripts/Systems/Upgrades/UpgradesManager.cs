using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class UpgradesManager : MonoBehaviour
{
    private List<UpgradeEntity> upgrades;
    public event Action<UpgradeEntity> UpgradeChoosen = delegate { };


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
            SetUpgradeButtons();
        }
    }

    private void GenerateUpgrades()
    {
        upgrades = new List<UpgradeEntity>();

        UpgradeEntity upgrade1 = new PlayerSpeedUpgrade();
        upgrade1.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade1);

        UpgradeEntity upgrade2 = new HealthUpgrade();
        upgrade2.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade2);

        UpgradeEntity upgrade3 = new BaseWeaponUpgrade();
        upgrade3.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade3);

        UpgradeEntity upgrade4 = new BaseWeaponUpgrade();
        upgrade4.OnUpgradeChoice += HandleUpgradeChoice;
        upgrades.Add(upgrade4);
    }

    private void SetUpgradeButtons()
    {
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

    private void OnEnable()
    {
        UpgradeChoosen += OnUpgradeChoice;
    }

    private void OnDisable()
    {
        UpgradeChoosen -= OnUpgradeChoice;
    }

    private void OnUpgradeChoice(UpgradeEntity upgrade)
    {
        if (upgrade.exhausted)
        {
            upgrades.Remove(upgrade);
        }
        GameManager.Instance.ChangeState(GameState.Game);
    }

    private void HandleUpgradeChoice(UpgradeEntity upgrade)
    {
        UpgradeChoosen(upgrade);
    }
}

public abstract class UpgradeEntity
{
    public bool exhausted;
    public abstract string upgradeText { get; }
    public Action<UpgradeEntity> OnUpgradeChoice;

    public abstract void UpgradeFunction();
}
