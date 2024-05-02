using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthController
{
    [SerializeField] private UIController playerHUD;
    public event Action<int> Damaged = delegate { };
    public event Action Killed = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        playerHUD.SetMaxHealth(maxHealth);
        Killed += LevelManager.Instance.GameOver;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        playerHUD.SetHealth(health);
        Damaged.Invoke(health);
    }

    protected override void Kill()
    {
        base.Kill();
        Killed.Invoke();
        GetComponent<PlayerController>().DisablePlayer();
    }

    public override void SetHealthToMax()
    {
        base.SetHealthToMax();
        playerHUD.SetHealth(health);
    }
}
