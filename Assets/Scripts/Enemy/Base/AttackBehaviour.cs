using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    private IAttackPattern attackPattern;
    BaseEnemy baseEnemy;

    private void Start()
    {
        baseEnemy = GetComponentInParent<BaseEnemy>();
    }
    public void SetAttackPattern(IAttackPattern newPattern)
    {
        attackPattern = newPattern;
    }

    public void TryAttack()
    {
        if (attackPattern != null && attackPattern.CanAttack())
        {
            baseEnemy.FlipObject(baseEnemy.GetTarget().transform);
            baseEnemy.IsAttacking = true;
            //Debug.Log("Call Try Attack");
            if (baseEnemy.haveRun)
            {
                baseEnemy.animator.SetBool("IsRun", false);
            }
            attackPattern.ExecuteAttack();
        }
    }
}

