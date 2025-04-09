using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private float cooldownTime;
    [SerializeField] private float comboTime;
    [SerializeField] private int maxComboCount;
    PlayerController player;
    public override float CooldownTime => cooldownTime;
    public override float ComboTime => comboTime;
    public override int comboAttackCount => maxComboCount;
    
    public override void AttachAnimator()
    {
        baseAnimator = transform.Find("Base")?.GetComponent<Animator>();
        weaponSpriteAnimator = transform.Find("WeaponSprite")?.GetComponent<Animator>();
        WeaponVFXAnimator = transform.Find("WeaponVFX")?.GetComponent<Animator>();

        if (baseAnimator == null)
            Debug.LogError("Không tìm thấy Animator của Base!");

        if (weaponSpriteAnimator == null)
            Debug.LogError("Không tìm thấy Animator của WeaponSprite!");

        if(WeaponVFXAnimator == null)
        {
            Debug.LogError("Không tìm thấy Animator của WeaponVFXAnimator!");
        }
    }


    public override void Attack(int comboAttackIndex, Animator base_animator, Animator weaponSprite_animator, Animator weaponSFX_animator)
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerController>();
        }
        //Debug.Log("Sword Attack");
        switch (comboAttackIndex)
        {
            case 1:
                base_animator.Play("Chop_Base");
                weaponSprite_animator.Play("Chop");
                weaponSFX_animator.Play("Chop");

                break;
            case 2:
                base_animator.Play("Slash_Base");
                weaponSprite_animator.Play("Slash");
                weaponSFX_animator.Play("Slash");
                break;
            case 3:
                base_animator.Play("Slash_Rising_Base");
                weaponSprite_animator.Play("Slash_Rising");
                weaponSFX_animator.Play("Slash_Rising");
                break;
        }
    }
}
