using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] private HealthController healthController;

    private void Start()
    {
        SetMaxHealth(healthController.maxHealth);
        healthController.Damaged += SetHealth;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetHealthToMax()
    {
        healthController.SetHealthToMax();
        slider.value = healthController.maxHealth;
    }

}
