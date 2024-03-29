using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthController
{
    public event Action<int> Damaged = delegate { };
    public event Action Killed = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Damaged.Invoke(health);
    }

    protected override void Kill()
    {
        Killed.Invoke();
        base.Kill();
    }
}
