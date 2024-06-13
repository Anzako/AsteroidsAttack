
using UnityEngine;

public class PlayerHealth : HealthController, IHealable
{
    [SerializeField] private UIController playerHUD;

    // Sounds
    [SerializeField] private AudioClip gettingHitSoundClip;

    private int initialHealth;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        Killed += LevelManager.Instance.EndGame;
        initialHealth = GetMaxHealth();
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
        SoundFXManager.Instance.PlaySoundFXClip(gettingHitSoundClip, transform, 1f);
        playerHUD.SetHealth(Health);
    }

    protected override void Kill()
    {
        base.Kill();
    }

    public override void SetHealthToMax()
    {
        base.SetHealthToMax();
        playerHUD.SetMaxHealth(Health);
    }

    public void Heal(int healAmount)
    {
        AddHealth(healAmount);
        playerHUD.SetHealth(Health);
    }

    public void IncreaseHealth(int amount)
    {
        SetMaxHealth(Health + amount);
        SetHealthToMax();
    }

    public void ResetStats()
    {
        SetMaxHealth(initialHealth);
        SetHealthToMax();
    }
}
