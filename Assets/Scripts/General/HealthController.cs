using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamagable
{
    public event Action Damaged = delegate { };
    public event Action Killed = delegate { };

    // Health
    [SerializeField] private int maxHealth;
    [field: SerializeField] public int health { get; private set; }

    private void Awake()
    {
        SetHealthToMax();
    }

    public void SetHealth(int healthAmount)
    {
        health = healthAmount;
    }

    public virtual void SetHealthToMax()
    {
        SetHealth(maxHealth);
    }

    public virtual void Damage(int damageAmount)
    {
        health -= damageAmount;
        Damaged?.Invoke();
        if (health <= 0)
        {
            Kill();
        }
    }

    protected virtual void Kill()
    {
        Killed?.Invoke();
    }
}


public interface IDamagable
{
    public void Damage(int damageAmount);
}
