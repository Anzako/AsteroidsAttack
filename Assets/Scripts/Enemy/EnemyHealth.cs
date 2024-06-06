using UnityEngine;

public class EnemyHealth : HealthController
{
    public override void Damage(int damage)
    {
        base.Damage(damage);
    }

    protected override void Kill()
    {
        base.Kill();
    }
}
