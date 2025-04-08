using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeHandler : MonoBehaviour
{
    AttackRangeDetector attackRangeDetector;
    ChasingBehavior chasingBehaviour;
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.tag == "AttackRange")
        {
            attackRangeDetector = trigger.gameObject.GetComponentInParent<AttackRangeDetector>();
            if (attackRangeDetector == null ) { return; }
            attackRangeDetector.attackRange = trigger.name;
            attackRangeDetector.DetectRange();
        }
    }
    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (trigger.tag == "AttackRange")
        {
            attackRangeDetector = trigger.gameObject.GetComponentInParent<AttackRangeDetector>();
            if (attackRangeDetector == null) { return; }
            attackRangeDetector.attackRange = trigger.name;
            attackRangeDetector.DetectRange();
        }
        
    }
    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.tag == "AttackRange")
        {
            attackRangeDetector = trigger.gameObject.GetComponentInParent<AttackRangeDetector>();
            if (attackRangeDetector == null) { return; }
            attackRangeDetector.attackRange = trigger.name;
            attackRangeDetector.UnDetectRange();
        }
    }


}
