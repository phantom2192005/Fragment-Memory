using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Stamina Regen", fileName = "StaminaRegenModSO")]
public class StaminaRegenModSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject target, float val)
    {
        Stamina stamina = target.GetComponent<Stamina>();
        if (stamina != null)
        {
            stamina.ModifyStamina(val); // Bạn có typo ở đây nè!
        }
    }
}
