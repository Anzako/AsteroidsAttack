using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreOnComboText;
    [SerializeField] private TMP_Text comboText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text asteroidsAmountText;

    private void Start()
    {
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
        scoreText.gameObject.SetActive(isActive);
        healthSlider.gameObject.SetActive(isActive);
        waveText.gameObject.SetActive(isActive);
        asteroidsAmountText.gameObject.SetActive(isActive);
    }


}
