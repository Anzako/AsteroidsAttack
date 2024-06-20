using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChoiceMenu : Singleton<UI_ChoiceMenu>
{
    private Transform choiceButton1;
    private Transform choiceButton2;
    private Transform choiceButton3;
    private Transform bossUpgradeButton;

    protected override void Awake()
    {
        base.Awake();
        choiceButton1 = transform.Find("ChoiceButton1");
        choiceButton2 = transform.Find("ChoiceButton2");
        choiceButton3 = transform.Find("ChoiceButton3");
        bossUpgradeButton = transform.Find("BossUpgradeButton");
    }

    public void SetButton1(UpgradeEntity upgrade)
    {
        choiceButton1.GetComponentInChildren<TMP_Text>().text = upgrade.upgradeText;
        choiceButton1.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceButton1.GetComponent<Button>().onClick.AddListener(upgrade.UpgradeFunction);
    }

    public void SetButton2(UpgradeEntity upgrade)
    {
        choiceButton2.GetComponentInChildren<TMP_Text>().text = upgrade.upgradeText;
        choiceButton2.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceButton2.GetComponent<Button>().onClick.AddListener(upgrade.UpgradeFunction);
    }

    public void SetButton3(UpgradeEntity upgrade)
    {
        choiceButton3.GetComponentInChildren<TMP_Text>().text = upgrade.upgradeText;
        choiceButton3.GetComponent<Button>().onClick.RemoveAllListeners();
        choiceButton3.GetComponent<Button>().onClick.AddListener(upgrade.UpgradeFunction);
    }

    public void SetUpgradeButton(UpgradeEntity upgrade)
    {
        bossUpgradeButton.GetComponentInChildren<TMP_Text>().text = upgrade.upgradeText;
        bossUpgradeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        bossUpgradeButton.GetComponent<Button>().onClick.AddListener(upgrade.UpgradeFunction);
    }

    public void DisableButton1()
    {
        choiceButton1.gameObject.SetActive(false);
    }

    public void DisableButton2()
    {
        choiceButton2.gameObject.SetActive(false);
    }

    public void DisableButton3()
    {
        choiceButton3.gameObject.SetActive(false);
    }

    public void DisableBossUpgradeButton()
    {
        bossUpgradeButton.gameObject.SetActive(false);
    }

    public void EnableButton1()
    {
        choiceButton1.gameObject.SetActive(true);
    }

    public void EnableButton2()
    {
        choiceButton2.gameObject.SetActive(true);
    }

    public void EnableButton3()
    {
        choiceButton3.gameObject.SetActive(true);
    }

    public void EnableBossUpgradeButton()
    {
        bossUpgradeButton.gameObject.SetActive(true);
    }
}
