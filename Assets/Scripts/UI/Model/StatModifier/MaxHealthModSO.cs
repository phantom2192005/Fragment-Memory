using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStatModifierSO/Max Health", fileName = "MaxHealthModSO")]
public class MaxHealthModSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject target, float val)
    {
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.maxHealth += (int)val;
            health.ModifyHealth(health.maxHealth - health.currentHealth);
        }
    }
}
