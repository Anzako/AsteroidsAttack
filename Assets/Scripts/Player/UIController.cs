using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject userInterface;
    private Slider healthSlider;
    private TMP_Text scoreText;
    private TMP_Text scoreOnComboText;
    private TMP_Text comboText;
    private TMP_Text waveText;
    private TMP_Text asteroidsAmountText;

    private void Awake()
    {
        healthSlider = userInterface.transform.Find("HealthBar").GetComponent<Slider>();
        scoreText = userInterface.transform.Find("Score").GetComponent<TMP_Text>();
        scoreOnComboText = userInterface.transform.Find("ScoreOnCombo").GetComponent<TMP_Text>();
        comboText = userInterface.transform.Find("Combo").GetComponent<TMP_Text>();
        waveText = userInterface.transform.Find("Wave").GetComponent<TMP_Text>();
        asteroidsAmountText = userInterface.transform.Find("AsteroidsLeft").GetComponent<TMP_Text>();

        scoreText.text = "Score: 0";
    }

    public void SetAsteroidsAmountText(int amount)
    {
        asteroidsAmountText.text = "Asteroids: " + amount;
    }

    public void SetWave(int wave)
    {
        waveText.text = "Wave " + wave;
    }

    public void SetMaxHealth(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public void SetHealth(int health)
    {
        healthSlider.value = health;
    }


    public void SetScore(int score)
    {
        scoreText.text = GetScoreText(score);
    }

    public void SetScoreOnCombo(int score)
    {
        scoreOnComboText.text = score.ToString();
    }

    public void SetCombo(int value)
    {
        comboText.text = "x" + value;
    }

    private string GetScoreText(int score)
    {
        string text = "Score: " + score;
        return text;
    }

    public void SetActive(bool isActive) 
    {
        userInterface.SetActive(isActive);
    }


}
