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
            float newMaxHealth = health.maxHealth + val;
            float delta = newMaxHealth - health.currentHealth;
            health.maxHealth = newMaxHealth;
            health.ModifyHealth(delta);
        }
    }
}
