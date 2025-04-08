using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStatModifierSO/Max Stamina", fileName = "MaxStaminaModSO")]
public class MaxStaminaModSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject target, float val)
    {
        Stamina stamina = target.GetComponent<Stamina>();
        if (stamina != null)
        {
            stamina.MaxStamina += (int)val;
            stamina.ModifyStamina(stamina.MaxStamina - stamina.GetCurrentStamia());
        }
    }
}
