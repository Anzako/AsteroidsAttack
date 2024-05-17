
public class PlayerHealth : HealthController
{
    private UIController playerHUD;

    private void Awake()
    {
        playerHUD = GetComponent<UIController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHealthToMax();
        Killed += LevelManager.Instance.EndGame;
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
        playerHUD.SetHealth(health);
    }

    protected override void Kill()
    {
        base.Kill();
        //GetComponent<PlayerController>().DisablePlayer();
    }

    public override void SetHealthToMax()
    {
        base.SetHealthToMax();
        playerHUD.SetMaxHealth(health);
    }
}
