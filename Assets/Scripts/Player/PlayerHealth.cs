
using UnityEngine;

public class PlayerHealth : HealthController
{
    [SerializeField] private UIController playerHUD;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    public override void SetHealthToMax()
    {
        base.SetHealthToMax();
        playerHUD.SetMaxHealth(health);
    }
}
