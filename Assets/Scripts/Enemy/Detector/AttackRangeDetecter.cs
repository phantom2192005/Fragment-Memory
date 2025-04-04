
using UnityEngine;
using System;

public class AttackRangeDetector : MonoBehaviour
{
    public bool inAttackRange;
    public bool inMeleeRange;
    public bool inRangedRange;
    public string attackRange;
    public AttackBehaviour attackBehaviour;
    EnemeyController enemyController;

    private void Start()
    {
        enemyController = GetComponentInParent<EnemeyController>();
    }
    public void DetectRange()
    {
        if (attackRange == "Melee Range")
        {
            Debug.Log("In melee range");
            inMeleeRange = true;
            inRangedRange = false;
            inAttackRange = true;
            if (enemyController.haveRun) { enemyController.animator.SetBool("IsRun", false); }
        }
        if (attackRange == "Ranged Range" && inMeleeRange == false)
        {
            inRangedRange = true;
            inAttackRange = true;
            if (enemyController.haveRun) { enemyController.animator.SetBool("IsRun", false); }
        }
    }

    public void UnDetectRange()
    {
        if (attackRange == "Melee Range")
        {
            inMeleeRange = false;
            inAttackRange = false;
            if (enemyController.haveRun) { enemyController.animator.SetBool("IsRun", true); }
        }
        if (attackRange == "Ranged Range")
        {
            inRangedRange = false;
            inAttackRange = false;
            if (enemyController.haveRun) { enemyController.animator.SetBool("IsRun", true); }
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
