using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private GameObject currentWeaponPrefab;
    private Weapon currentWeapon;
    [Header("Animators")]
    [SerializeField] private GameObject Base;
    [SerializeField] private GameObject WeaponSprite;
    [SerializeField] private GameObject WeaponVFX;
    [SerializeField] private GameObject HitBox;

    private SpriteRenderer weaponSprite_spriteRenderer;

    private Animator base_Animator; // exsit in the prefab
    private Animator weaponSprite_Animator;
    private Animator weaponVFX_Animator;

    [SerializeField]
    private EquippableItemSO weapon;

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;



    private PlayerController player;

    void Start()
    {
        if(currentWeaponPrefab == null) { return; }
        PrepareAnimators();
    }

    void PrepareAnimators()
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
        weaponVFX_Animator = WeaponVFX != null ? WeaponVFX.GetComponent<Animator>() : null;

        // set up hitBox
        WeaponVFX.GetComponent<HitBoxHandler>().hitBoxPrefabs = currentWeapon.hitBoxes;
        WeaponVFX.GetComponent<HitBoxHandler>().DestroyAllHitBoxs(HitBox.transform);
        WeaponVFX.GetComponent<HitBoxHandler>().InitializeHitBoxes(HitBox.transform);


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
        if (currentWeaponPrefab == null) { return; }
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

    public void EquipWeapon(GameObject newWeapon, EquippableItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        Debug.Log("Equip weapon");

        if (weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrentState);
        }

        this.weapon = weaponItemSO;
        this.itemCurrentState = itemState;

        currentWeaponPrefab = newWeapon;
        PrepareAnimators();
        ModifyParameters(parametersToModify);
    }
    private void ModifyParameters(List<ItemParameter> parametersToModify)
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                float newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
