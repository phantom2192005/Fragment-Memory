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

    // Gizmo parameters
    public Vector2 targetPositionCached;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, ContextData contextData)
    {
        // Nếu không có mục tiêu, dừng tìm kiếm
        if (reachedLastTarget)
        {
            if (contextData.targets == null || contextData.targets.Count <= 0)
            {
                contextData.currentTarget = null;
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

        // Lưu vị trí cuối cùng của mục tiêu nếu vẫn còn thấy nó
        if (contextData.currentTarget != null && contextData.targets != null && contextData.targets.Contains(contextData.currentTarget))
            targetPositionCached = contextData.currentTarget.position;

        // Kiểm tra xem đã đến mục tiêu chưa
        if (Vector2.Distance(transform.position, targetPositionCached) < targetRechedThreshold)
        {
            reachedLastTarget = true;
            contextData.currentTarget = null;
            return (danger, interest);
        }

        // Nếu chưa đến mục tiêu, xác định hướng đi
        Vector2 directionToTarget = (targetPositionCached - (Vector2)transform.position);
        if (targetPositionCached != null)
        {
            baseEnemy.FlipObject(targetPositionCached);
        }

        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            // Chỉ chấp nhận hướng có góc < 90 độ với hướng mục tiêu
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
        if (!showGizmo)
            return;

        Gizmos.DrawSphere(targetPositionCached, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < interestsTemp.Length; i++)
            {
                Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestsTemp[i] * 2);
            }
            if (!reachedLastTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(targetPositionCached, 0.1f);
            }
        }
    }
}
