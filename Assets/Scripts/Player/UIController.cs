using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        scoreText.text = "Score: 0";
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

    private string GetScoreText(int score)
    {
        string text = "Score: " + score;
        return text;
    }

    public void SetActive(bool isActive) 
    {
        scoreText.gameObject.SetActive(isActive);
        healthSlider.gameObject.SetActive(isActive);
    }


}
