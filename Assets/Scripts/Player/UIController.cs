using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject userInterface;
    private Image healthSlider;
    private TMP_Text scoreText;
    private TMP_Text scoreOnComboText;
    private TMP_Text comboText;
    private TMP_Text waveText;
    private TMP_Text taskText;

    private void Awake()
    {
        healthSlider = userInterface.transform.Find("HealthBar").Find("Fill").GetComponent<Image>();
        scoreText = userInterface.transform.Find("Score").GetComponent<TMP_Text>();
        scoreOnComboText = userInterface.transform.Find("ScoreOnCombo").GetComponent<TMP_Text>();
        comboText = userInterface.transform.Find("Combo").GetComponent<TMP_Text>();
        waveText = userInterface.transform.Find("Wave").GetComponent<TMP_Text>();
        taskText = userInterface.transform.Find("TaskText").GetComponent<TMP_Text>();

        scoreText.text = "0";
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


}
