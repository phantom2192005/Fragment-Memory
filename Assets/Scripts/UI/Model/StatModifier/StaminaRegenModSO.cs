using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStatModifierSO/Stamina Regen", fileName = "StaminaRegenModSO")]
public class StaminaRegenModSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject target, float val)
    {
        Stamina stamina = target.GetComponent<Stamina>();
        if (stamina != null)
        {
            stamina.ModifyStamina((int)val);
        }
    }
}
