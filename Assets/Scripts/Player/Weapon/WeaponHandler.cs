using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject currentWeaponPrefab; // Đã lưu trữ animator của Base
    private Weapon currentWeapon;
    [SerializeField] private GameObject Base;
    [SerializeField] private GameObject WeaponSprite;
    [SerializeField] private GameObject WeaponSFX;
    [SerializeField] private GameObject HitBox;

    private SpriteRenderer weaponSprite_spriteRenderer;

    private Animator base_Animator; // exsit in the prefab
    private Animator weaponSprite_Animator;
    private Animator weaponVFX_Animator;




    private PlayerController player;

    void Start()
    {
        weaponSprite_spriteRenderer = WeaponSprite.GetComponent<SpriteRenderer>();
        player = FindFirstObjectByType<PlayerController>();

        if (currentWeaponPrefab != null)
        {
            currentWeapon = currentWeaponPrefab.GetComponent<Weapon>();
        }

        //set up Animator
        base_Animator = Base != null ? Base.GetComponent<Animator>() : null;
        weaponSprite_Animator = WeaponSprite != null ? WeaponSprite.GetComponent<Animator>() : null;
        weaponVFX_Animator = WeaponSFX != null ? WeaponSFX.GetComponent<Animator>() : null;

        // set up hitBox
        WeaponSFX.GetComponent<HitBoxHandler>().hitBoxPrefabs = currentWeapon.hitBoxes;
        WeaponSFX.GetComponent<HitBoxHandler>().InitializeHitBoxes(HitBox.transform);


        currentWeaponPrefab.GetComponent<Weapon>().AttachAnimator();

        if (currentWeapon != null)
        {
            if (currentWeapon.baseAnimator != null && base_Animator != null)
            {
                //Debug.Log("Attempt to attach base animator");
                base_Animator.runtimeAnimatorController = currentWeapon.baseAnimator.runtimeAnimatorController;
            }

            if (currentWeapon.weaponSpriteAnimator != null && weaponSprite_Animator != null)
            {
                //Debug.Log("Attempt to attach weaponSprite animator");
                weaponSprite_Animator.runtimeAnimatorController = currentWeapon.weaponSpriteAnimator.runtimeAnimatorController;
            }

            if (currentWeapon.WeaponVFXAnimator != null && weaponVFX_Animator != null)
            {
                //Debug.Log("Attempt to attach weaponSprite_Animator");
                weaponVFX_Animator.runtimeAnimatorController = currentWeapon.WeaponVFXAnimator.runtimeAnimatorController;
            }
        }
    }

    private void Update()
    {
        if (weaponSprite_Animator == null)
        {
            Debug.Log("WeaponSprite_Animator is null");

            return;
        }
        else if (base_Animator == null)
        {
            Debug.Log("base_Animator is null");
        }
        // set aniamtion direction
        base_Animator.SetFloat("InputX", player.GetAnimator().GetFloat("InputX"));
        base_Animator.SetFloat("InputY", player.GetAnimator().GetFloat("InputY"));

        weaponSprite_Animator.SetFloat("InputX", player.GetAnimator().GetFloat("InputX"));
        weaponSprite_Animator.SetFloat("InputY", player.GetAnimator().GetFloat("InputY"));

        weaponVFX_Animator.SetFloat("InputX", player.GetAnimator().GetFloat("InputX"));
        weaponVFX_Animator.SetFloat("InputY", player.GetAnimator().GetFloat("InputY"));

        if (player.GetAnimator().GetFloat("InputY") == 1)
        {
            weaponSprite_spriteRenderer.sortingOrder = 0;
        }
        else
        {
            weaponSprite_spriteRenderer.sortingOrder = 1;
        }
        switch (player.currentState)
        {
            case PlayerIdleState:
                weaponSprite_Animator.Play("Idle");
                break;

            case PlayerWalkState:
                weaponSprite_Animator.Play("Walk");
                break;

            case PlayerRunState:
                weaponSprite_Animator.Play("Run");
                break;

            case PlayerRollState:
                weaponSprite_Animator.Play("Roll");
                break;

            case PlayerDeathState:

                break;
        }
    }


    public Weapon GetWeapon()
    {
        if (currentWeapon == null)
        {
            Debug.Log("No weapon is attach");
        }
        return currentWeapon;
    }

    public void Attack(int comboAttackIndex)
    {
        //Debug.Log("Attack from WeaponHandler is called");

        if (currentWeapon != null)
        {
            currentWeapon.Attack(comboAttackIndex, base_Animator, weaponSprite_Animator, weaponVFX_Animator);
        }
        else
        {
            Debug.LogWarning("WeaponHandler: Không có vũ khí để tấn công!");
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        weaponSprite_Animator = newWeapon.GetComponent<Animator>();

        // Gán animator của vũ khí mới vào Base
        if (base_Animator != null && weaponSprite_Animator != null)
        {
            base_Animator.runtimeAnimatorController = weaponSprite_Animator.runtimeAnimatorController;
        }
        else
        {
            Debug.LogWarning("Vũ khí mới không có Animator hoặc Base không có Animator!");
        }

        Debug.Log("Đã trang bị vũ khí mới: " + newWeapon.name);
    }
}
