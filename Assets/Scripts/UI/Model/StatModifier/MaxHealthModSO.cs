using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Max Health", fileName = "MaxHealthModSO")]
public class MaxHealthModSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject target, float val)
    {
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            int newMaxHealth = health.maxHealth + (int)val;
            int delta = newMaxHealth - health.currentHealth;
            health.maxHealth = newMaxHealth;
            health.ModifyHealth(delta);
        }
    }
}
