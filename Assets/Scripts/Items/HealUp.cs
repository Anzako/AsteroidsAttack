using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealUp : MonoBehaviour
{
    public int healAmount;

    private void OnCollisionEnter(Collision collision)
    {
        IHealable healable = collision.gameObject.GetComponentInParent<IHealable>();

        if (healable != null )
        {
            healable.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}
