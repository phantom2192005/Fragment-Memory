using UnityEngine;

public class EnemeyController : MonoBehaviour
{
    [Header("State Management")]
    public bool isPatrolling = true;
    public bool isIdling;
    public bool isDead;
    public bool isAttacking;

    [Header("Enemy Option")]
    public bool haveRun;


    [Header("References")]
    private PatrolBehaviour patrol;
    private ChasingBehavior chase;
    public AttackBehaviour attack;
    private GameObject target;
    public Animator animator;
    public TargetDetector targetDetecter;
    public AttackRangeDetector attackRangeDetector;
    public GameObject HurtBox;
    public Health health;

    void Awake()
    {
        patrol = GetComponent<PatrolBehaviour>();
        chase = GetComponent<ChasingBehavior>();
        attack = GetComponentInChildren<AttackBehaviour>();
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        attackRangeDetector = GetComponentInChildren<AttackRangeDetector>();
        HurtBox = transform.Find("HurtBox")?.gameObject;
        health = HurtBox.GetComponent<Health>();
    }


    public GameObject GetTarget()
    {
        return target;
    }

    void Update()
    {
        if (health.currentHealth <=0 )
        {
            HandlerAfterDeath();
        }
        if (isDead) return;
        if (targetDetecter.isPlayerDetected == true)
        {
            isPatrolling = false;
        }
        if (targetDetecter.isPlayerDetected && attackRangeDetector.inAttackRange == false && isAttacking == false)
        {
            chase.ChasePlayer();
        }
        else if (targetDetecter.isPlayerDetected == false)
        {
            isPatrolling = true;
        }
        if (isPatrolling)
        {
            patrol.Patrol();
        }
    }

    public void FlipObject(Vector3 targetPosition)
    {
        if (targetPosition == null || isDead) return;

        Vector3 scale = transform.localScale;

        if (targetPosition.x < transform.position.x)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }


    public void SetAnimatorBoolParameter(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void HandlerAfterDeath()
    {
        isDead = true;
        animator.Play("Death");
        HurtBox.GetComponent<Collider2D>().enabled = false;
        HurtBox.GetComponent<DamageFlash>().enabled = false;
    }

    public void OnAnimationEnter()
    {
        isAttacking = true;
        if (haveRun) { animator.SetBool("IsRun", false); }
    }

    public void OnAnimationEnd(string nameBoolAnimator)
    {
        SetAnimatorBoolParameter(nameBoolAnimator, false);
        attack.SetAttackPattern(null);
        isAttacking = false;
    }

    public void FireProjectTile()
    {

        GetComponentInChildren<RangedAttack>().FireProjectile();

    }
}
