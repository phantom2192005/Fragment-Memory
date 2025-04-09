using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
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

        if (WeaponVFXAnimator == null)
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
        switch (comboAttackIndex)
        {
            case 1:
                //base_animator.Play("");
                base_animator.Play("Attack", 0, 0.0f);
                weaponSprite_animator.Play("Attack",0,0.0f);
                weaponSFX_animator.Play("Attack",0,0.0f);

                break;
        }
    }

}
