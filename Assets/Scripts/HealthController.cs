using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    // Health
    public event Action<int> Damaged = delegate { };
    public event Action Killed = delegate { };
    [SerializeField] private int health = 10;
    ParticleSystem onHitParticle;

    // Start is called before the first frame update
    void Start()
    {
        onHitParticle = GetComponentInChildren<ParticleSystem>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Damaged.Invoke(damage);
        onHitParticle.Play();

        if (health <= 0)
        {
            Kill();
        }

    }

    private void Kill()
    {
        Killed.Invoke();
        Destroy(this.gameObject);
    }
}
