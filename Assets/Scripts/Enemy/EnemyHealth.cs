using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthController
{
    public event Action Damaged = delegate { };
    public event Action<int> Killed = delegate { };
    [SerializeField] int score;

    // Start is called before the first frame update
    void Start()
    {
        //health = maxHealth;
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
        Damaged.Invoke();
    }

    protected override void Kill()
    {
        Killed.Invoke(score);
        base.Kill();
        Destroy(gameObject);
    }
}
