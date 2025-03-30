using System.Diagnostics;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    public bool inMeleeRange;
    public bool inRangedRange;
    public string attackRange;
    public AttackBehaviour attackBehaviour;
    BaseEnemy baseEnemy;

    private void Start()
    {
        baseEnemy = GetComponentInParent<BaseEnemy>();
    }
    public void DetectRange()
    {
        if (attackRange == "Melee Range")
        {
            inMeleeRange = true;
            inRangedRange = false;
            baseEnemy.IsAttacking = true;
            
        }
        if (attackRange == "Ranged Range" && inMeleeRange == false)
        {
            inRangedRange = true;
            baseEnemy.IsAttacking = true;
        }
    }

    public void UnDetectRange()
    {
        if (attackRange == "Melee Range")
        {
            inMeleeRange = false;
            //baseEnemy.IsAttacking = false;
            //baseEnemy.animator.SetBool("IsMeleeAttack", false);
        }
        if (attackRange == "Ranged Range")
        {
            inRangedRange = false;
            //baseEnemy.IsAttacking = false
        }
    }

    private void Update()
    {
        ChangeAttackPattern();
    }
    void ChangeAttackPattern()
    {
        if (inMeleeRange)
        {
            attackBehaviour.SetAttackPattern(attackBehaviour.GetComponent<MeleeAttack>());
            attackBehaviour.TryAttack();
        }
        else if (inRangedRange)
        {
            attackBehaviour.SetAttackPattern(attackBehaviour.GetComponent<RangedAttack>());
            attackBehaviour.TryAttack();
        }
        else
        {

        }
    }

}
