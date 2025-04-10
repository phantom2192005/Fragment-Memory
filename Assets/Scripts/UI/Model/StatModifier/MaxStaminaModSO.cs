using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Max Stamina", fileName = "MaxStaminaModSO")]
public class MaxStaminaModSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject target, float val)
    {
        Stamina stamina = target.GetComponent<Stamina>();
        if (stamina != null)
        {
            float newMaxStamina = stamina.maxStamina +  val;
            float delta = newMaxStamina - stamina.GetCurrentStamia();
            stamina.maxStamina = newMaxStamina;
            stamina.ModifyStamina(delta);
        }
    }
}
