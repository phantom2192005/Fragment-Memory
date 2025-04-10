using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Health Regen", fileName = "HealthRegenModSO")]
public class HealthRegenModSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject target, float val)
    {
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.ModifyHealth((int)val);
        }
    }
}

