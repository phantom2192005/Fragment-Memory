using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract float CooldownTime { get; }
    public abstract float ComboTime { get; }
    public abstract int comboAttackCount { get; }

    public Animator weaponSpriteAnimator;
    public Animator baseAnimator;
    public Animator WeaponVFXAnimator;

    [SerializeField] public List<GameObject> hitBoxes = new List<GameObject>();

    public abstract void AttachAnimator();
    public abstract void Attack(int comboAttackIndex, Animator base_animator, Animator weaponSprite_animator, Animator weaponSFX_animator);
}