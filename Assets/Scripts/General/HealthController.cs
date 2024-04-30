using UnityEngine;

public class HealthController : MonoBehaviour
{
    // Health
    public int maxHealth;
    protected int health;

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }
    }

    protected virtual void Kill()
    {
        
    }

    public virtual void SetHealthToMax()
    {
        health = maxHealth;
    }
}
