using UnityEngine;

public class EnemyHealth : HealthController
{
    [SerializeField] int score;

    public override void Damage(int damage)
    {
        base.Damage(damage);
    }

    protected override void Kill()
    {
        base.Kill();
        Destroy(gameObject);
    }
}
