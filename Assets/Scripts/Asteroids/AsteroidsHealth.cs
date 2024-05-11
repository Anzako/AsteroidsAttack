
public class AsteroidsHealth : HealthController
{
    private AsteroidController aController;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        SetHealthToMax();
        aController = GetComponent<AsteroidController>();

        Killed += aController.OnProjectileDestroy;
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
    }

    protected override void Kill()
    {
        base.Kill();
        ScoreManager.instance.AddScore(score);;
    }
}
