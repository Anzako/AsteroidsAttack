
using UnityEngine;

public class PlayerHealth : HealthController, IHealable
{
    [SerializeField] private UIController playerHUD;
    public bool immortal = false;

    // Shield
    private bool isShield;
    [SerializeField] private GameObject shieldObject;

    // Sounds
    [SerializeField] private AudioClip gettingHitSoundClip;
    [SerializeField] private AudioClip shieldHitSoundClip;

    private int initialHealth;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        initialHealth = GetMaxHealth();
    }

    private void Start()
    {
        Killed += LevelManager.Instance.EndGame;
    }


    public override void Damage(int damage)
    {
        if (isShield)
        {
            isShield = false;
            playerHUD.SetShield(0);
            SoundFXManager.Instance.PlaySoundFXClip(shieldHitSoundClip, transform, 1f);
            shieldObject.SetActive(false);
            return;
        }

        if (!immortal)
        {
            base.Damage(damage);
        }
       
        SoundFXManager.Instance.PlaySoundFXClip(gettingHitSoundClip, transform, 1f);
        playerHUD.SetHealth(Health, maxHealth);
    }

    protected override void Kill()
    {
        base.Kill();
    }

    public override void SetHealthToMax()
    {
        base.SetHealthToMax();
        playerHUD.SetHealth(Health, maxHealth);
    }

    public void Heal(int healAmount)
    {
        AddHealth(healAmount);
        playerHUD.SetHealth(Health, maxHealth);
    }

    public void IncreaseHealth(int amount)
    {
        SetMaxHealth(maxHealth + amount);
        SetHealthToMax();
    }

    public void ResetStats()
    {
        immortal = false;
        isShield = false;
        SetMaxHealth(initialHealth);
        SetHealthToMax();
    }

    public void AddShield()
    {
        isShield = true;
        //playerHUD.SetShield(1);
        shieldObject.SetActive(true);
    }
}
