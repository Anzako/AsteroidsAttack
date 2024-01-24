using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    // Health
    public event Action<int> Damaged = delegate { };
    public event Action Killed = delegate { };
    public int maxHealth;
    private int health;
    ParticleSystem onHitParticle;

    // Start is called before the first frame update
    void Start()
    {
        onHitParticle = GetComponentInChildren<ParticleSystem>();
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }

        Damaged.Invoke(health);
        onHitParticle.Play();
    }

    private void Kill()
    {
        Killed.Invoke();
        gameObject.SetActive(false);
    }

    public void SetHealthToMax()
    {
        health = maxHealth;
    }
}
