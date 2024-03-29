using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class AsteroidsHealth : HealthController
{
    private AsteroidController aController;
    public event Action Damaged = delegate { };
    public event Action Killed = delegate { };
    [SerializeField] int score;

    // Start is called before the first frame update
    void Start()
    {
        SetHealthToMax();
        aController = GetComponent<AsteroidController>();

        Killed += aController.OnObjectPooled;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        Damaged.Invoke();
    }

    protected override void Kill()
    {
        Killed.Invoke();
    }
}
