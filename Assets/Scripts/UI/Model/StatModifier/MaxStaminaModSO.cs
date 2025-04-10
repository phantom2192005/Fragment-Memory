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
            int newMaxStamina = (int)val;
            int delta = newMaxStamina - stamina.maxStamina;
            stamina.maxStamina = newMaxStamina;
            stamina.ModifyStamina(delta); // có thể thay bằng stamina.currentStamina = stamina.maxStamina; nếu muốn hồi đầy
        }
    }
}
