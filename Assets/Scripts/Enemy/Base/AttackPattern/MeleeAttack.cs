using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackPattern
{
    [SerializeField] private float coolDownTime = 2f;
    public float cooldownTimer;
    BaseEnemy baseEnemy;

    void Start()
    {
        cooldownTimer = 0;
        baseEnemy = GetComponentInParent<BaseEnemy>();
    }

    public bool CanAttack()
    {
        return cooldownTimer <= 0;
    }

    public void ExecuteAttack()
    {
        if (CanAttack())
        {
            Debug.Log("Melee Attack!");
            cooldownTimer = coolDownTime;
            baseEnemy.SetAnimatorBoolParameter("IsMeleeAttack", true);
        }
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
