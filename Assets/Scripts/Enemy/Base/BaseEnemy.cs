using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("State Management")]
    public bool isChasing;
    public bool isPatrolling = true;
    public bool isIdling;

    public bool isDead;
    public bool isAttacking;

    [Header("Enemy Option")]
    public bool haveRun;
    public bool IsChasing
    {
        get => isChasing;
        set
        {
            isChasing = value;
            if (value) isPatrolling = false;
        }
    }

    public bool IsPatrolling
    {
        get => isPatrolling;
        set
        {
            isPatrolling = value;
            if (value) isChasing = false;
        }
    }

    public bool IsIdling
    {
        get => isIdling;
        set => isIdling = value;
    }

    public bool IsAttacking
    {
        get => isAttacking;
        set
        {
            isAttacking = value;
            if (isAttacking) // Nếu đang attack, tắt các trạng thái khác
            {
                isIdling = false;
                isChasing = false;
                isPatrolling = false;
                if (haveRun)
                {
                    animator.SetBool("IsRun", false);
                }
            }
        }
    }

    public bool IsDead
    {
        get => isDead;
        set
        {
            isDead = value;
            if (isDead)
            {
                isChasing = false;
                isPatrolling = false;
                isIdling = false;
                isAttacking = false;
            }
        }
    }



    [Header("References")]
    private PatrolBehaviour patrol;
    private ChasingBehavior chase;
    private AttackBehaviour attack;
    private GameObject target;
    public Animator animator;

    void Awake()
    {
        patrol = GetComponent<PatrolBehaviour>();
        chase = GetComponent<ChasingBehavior>();
        attack = GetComponentInChildren<AttackBehaviour>();
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    public GameObject GetTarget()
    {
        return target;
    }

    void Update()
    {
        if (IsDead || IsAttacking) return;
        if(IsChasing)
        {   
            chase.ChasePlayer();
        }
        else if (isPatrolling)
        {
            patrol.Patrol();
        }
    }

    public void FlipObject(Transform targetToFlip)
    {
        if (targetToFlip == null) return;

        Vector3 scale = transform.localScale;
        scale.x = (targetToFlip.transform.position.x < transform.position.x) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    public void SetAnimatorBoolParameter(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void HandlerAfterDeath()
    {
        IsDead = true;
    }

    public void OnAnimationEnter()
    {
        
    }

    public void OnAnimationEnd(string nameBoolAnimator)
    {
        SetAnimatorBoolParameter(nameBoolAnimator, false);
        attack.SetAttackPattern(null);
    }

    public void FireProjectTile(string typeRangedAttack)
    {
        if (typeRangedAttack == "RangedAttack")
        {
            GetComponentInChildren<RangedAttack>().FireProjectile();
        }
    }
}
