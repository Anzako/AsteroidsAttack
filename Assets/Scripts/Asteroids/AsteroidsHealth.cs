
public class AsteroidsHealth : HealthController
{
    public int score;

    public override void Damage(int damage)
    {
        base.Damage(damage);
    }

    protected override void Kill()
    {
        base.Kill();
        ScoreManager.Instance.AddScore(score);;
    }
}
