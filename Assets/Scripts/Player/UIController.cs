using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Slider healthSlider;
    public TMP_Text scoreText;
    [SerializeField] private PlayerHealth healthController;

    private void Start()
    {
        scoreText.text = "Score: 0";
        SetMaxHealth(healthController.maxHealth);
        healthController.Damaged += SetHealth;
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

    public void SetHealthToMax()
    {
        healthController.SetHealthToMax();
        healthSlider.value = healthController.maxHealth;
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

    public void SetActiveUI(bool isActive) 
    { 
        scoreText.gameObject.SetActive(isActive);
        healthSlider.gameObject.SetActive(isActive);
    }


}
