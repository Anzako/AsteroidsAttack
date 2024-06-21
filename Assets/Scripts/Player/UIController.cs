using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject userInterface;
    private Image healthSlider;
    private Transform laserBar;
    private Transform rocketBar;
    private Image laserSlider;
    private Image rocketSlider;
    private TMP_Text scoreText;
    private TMP_Text scoreOnComboText;
    private TMP_Text comboText;
    private TMP_Text waveText;
    private TMP_Text taskText;

    private Transform shield1;
    private Transform shield2;
    private Transform shield3;

    private void Awake()
    {
        healthSlider = userInterface.transform.Find("HealthBar").Find("Fill").GetComponent<Image>();
        laserBar = userInterface.transform.Find("LaserBar");
        rocketBar = userInterface.transform.Find("RocketBar");
        laserSlider = laserBar.Find("Fill").GetComponent<Image>();
        rocketSlider = rocketBar.Find("Fill").GetComponent<Image>();
        shield1 = userInterface.transform.Find("Shield1");
        shield2 = userInterface.transform.Find("Shield2");
        shield3 = userInterface.transform.Find("Shield3");

        scoreText = userInterface.transform.Find("Score").GetComponent<TMP_Text>();
        scoreOnComboText = userInterface.transform.Find("ScoreOnCombo").GetComponent<TMP_Text>();
        comboText = userInterface.transform.Find("Combo").GetComponent<TMP_Text>();
        waveText = userInterface.transform.Find("Wave").GetComponent<TMP_Text>();
        taskText = userInterface.transform.Find("TaskText").GetComponent<TMP_Text>();

        scoreText.text = "0";
    }

    private void OnEnable()
    {
        laserBar.gameObject.SetActive(false);
        rocketBar.gameObject.SetActive(false);
        shield1.gameObject.SetActive(false);
        shield2.gameObject.SetActive(false);
        shield3.gameObject.SetActive(false);
    }

    public void SetTaskText(string text)
    {
        taskText.text = text;
    }

    public void SetWave(int wave)
    {
        waveText.text = "Wave: " + wave;
    }

    public void SetHealth(int health, int maxHealth)
    {
        float healthValue = (float) health / maxHealth;
        healthSlider.fillAmount = healthValue;
    }


    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void SetScoreOnCombo(int score)
    {
        scoreOnComboText.text = score.ToString();
    }

    public void SetCombo(int value)
    {
        comboText.text = "x" + value;
    }

    public void SetActive(bool isActive) 
    {
        userInterface.SetActive(isActive);
    }

    public void SetLaser(float amount, float maxAmount)
    {
        float laserValue = amount / maxAmount;
        laserSlider.fillAmount = laserValue;
    }

    public void SetRocket(float amount, float maxAmount)
    {
        float rocketValue = amount / maxAmount;
        rocketSlider.fillAmount = rocketValue;
    }

    public void EnableLaserSlider() 
    { 
        laserBar.gameObject.SetActive(true);
        rocketBar.gameObject.SetActive(false);
    }

    public void EnableRocketSlider()
    {
        laserBar.gameObject.SetActive(false);
        rocketBar.gameObject.SetActive(true);
    }

    public void DisableWeaponSliders()
    {
        laserBar.gameObject.SetActive(false);
        rocketBar.gameObject.SetActive(false);
    }

    public void SetShield(int activeShields)
    {
        if (activeShields == 0)
        {
            shield1.gameObject.SetActive(false);
            shield2.gameObject.SetActive(false);
            shield3.gameObject.SetActive(false);
        } else if (activeShields == 1)
        {
            shield1.gameObject.SetActive(true);
            shield2.gameObject.SetActive(false);
            shield3.gameObject.SetActive(false);
        } else if (activeShields == 2)
        {
            shield1.gameObject.SetActive(true);
            shield2.gameObject.SetActive(true);
            shield3.gameObject.SetActive(false);
        } else
        {
            shield1.gameObject.SetActive(true);
            shield2.gameObject.SetActive(true);
            shield3.gameObject.SetActive(true);
        }
    }

}
