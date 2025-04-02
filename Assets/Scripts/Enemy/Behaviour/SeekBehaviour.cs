using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private BaseEnemy baseEnemy;

    [SerializeField]
    private float targetRechedThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    bool reachedLastTarget = true;

    //gizmo parameters
    public Vector2 targetPositionCached;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, ContextData contextData)
    {
        //if we don't have a target stop seeking player and seek partrol point
        //else set a new target
        if (reachedLastTarget)
        {
            if (contextData.targets == null || contextData.targets.Count <= 0)
            {
                // Nếu không còn target thì chuyển sang AlternativeTarget
                baseEnemy.IsPatrolling = true;
                baseEnemy.IsPlayerDetected = false;
                return (danger, interest);
            }
            else
            {
                reachedLastTarget = false;
                contextData.currentTarget = contextData.targets
                    .OrderBy(target => Vector2.Distance(target.position, transform.position))
                    .FirstOrDefault();
            }
        }


        //cache the last position only if we still see the target (if the targets collection is not empty)
        if (contextData.currentTarget != null && contextData.targets != null && contextData.targets.Contains(contextData.currentTarget))
            targetPositionCached = contextData.currentTarget.position;

        //First check if we have reached the target
        if (Vector2.Distance(transform.position, targetPositionCached) < targetRechedThreshold)
        {
            reachedLastTarget = true;
            contextData.currentTarget = null;
            return (danger, interest);
        }

        //If we havent yet reached the target do the main logic of finding the interest directions
        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);
        if (targetPositionCached != null)
        {
            baseEnemy.FlipObject(targetPositionCached);
        }

        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            //accept only directions at the less than 90 degrees to the target direction
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }

            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }

    private void OnDrawGizmos()
    {

        if (showGizmo == false)
            return;
        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPositionCached, 0.1f);
                }
            }
        }
    }
}