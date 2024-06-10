
public class AsteroidsHealth : HealthController
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
