using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    private IAttackPattern attackPattern;
    EnemeyController baseEnemy;
    public EnemyMovementAI enemyMovementAI;


    private void Start()
    {
        baseEnemy = GetComponentInParent<EnemeyController>();
    }
    public void SetAttackPattern(IAttackPattern newPattern)
    {
        attackPattern = newPattern;
    }

    private void Update()
    {
        
    }

    public void TryAttack()
    {
        if (attackPattern != null && attackPattern.CanAttack() && enemyMovementAI.contextData.currentTarget != null)
        {
            baseEnemy.FlipObject(baseEnemy.GetTarget().transform.position);
            if (baseEnemy.haveRun)
            {
                baseEnemy.animator.SetBool("IsRun", false);
            }
            attackPattern.ExecuteAttack();
        }
    }
}

