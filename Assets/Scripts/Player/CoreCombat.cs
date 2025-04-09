using UnityEngine;
using UnityEngine.InputSystem;

public class CoreCombat : MonoBehaviour
{
    private Animator animator;

    [SerializeField] public WeaponHandler currentWeaponHandler;

    public float comboTimer;
    public float coolDownTimer;
    public int comboAttackIndex;

    public bool isCooldown;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (coolDownTimer > 0 && isCooldown)
        {
            coolDownTimer -= Time.deltaTime;
            if (coolDownTimer <= 0)
            {
                coolDownTimer = 0;
                isCooldown = false;
            }
        }

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0)
            {
                comboTimer = 0;
            }
        }
        else
        {
            comboAttackIndex = 1; // Reset attack index if combo time has expired
        }
    }

    public void TriggerCoolDown()
    {
        isCooldown = true;
    }

    public void Attack()
    {

        if (isCooldown == true) { return; }

        //Debug.Log("Attack in CoreCombat is call");
        currentWeaponHandler.Attack(comboAttackIndex);

        coolDownTimer = currentWeaponHandler.GetWeapon().CooldownTime;

        if (comboAttackIndex == 1)
        {
            comboTimer = currentWeaponHandler.GetWeapon().ComboTime;
        }

        comboAttackIndex++;

        if (comboAttackIndex > currentWeaponHandler.GetWeapon().comboAttackCount)
        {
            comboAttackIndex = 1;
            comboTimer = 0;
        }
    }
}