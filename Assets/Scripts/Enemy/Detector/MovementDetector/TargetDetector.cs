using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 5;
    [SerializeField]
    private Transform patrolSpot;
    [SerializeField]
    private float maxChaseDistance;
    [SerializeField]
    private float currentDistance;

    [SerializeField]
    private LayerMask obstaclesLayerMask, playerLayerMask;

    public bool isPlayerDetected;

    [SerializeField]
    private bool showGizmos = false;

    // Gizmo parameters
    private List<Transform> colliders;

    // Biến kiểm soát việc delay
    
    public bool canDetect = true;
    [SerializeField]
    private float detectionCooldown = 1.5f; // Khoảng thời gian chờ

    public override void Detect(ContextData aiData)
    {
        if (!canDetect) return; // Nếu đang delay thì không tiếp tục Detect

        currentDistance = Vector2.Distance(transform.position, patrolSpot.position);
        if (currentDistance > maxChaseDistance)
        {
            isPlayerDetected = false;
            StartCoroutine(DetectionCooldown()); // Gọi coroutine delay Detect
            return;
        }

        Collider2D playerCollider =
            Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider != null)
        {
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit =
                Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstaclesLayerMask);

            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                float thickness = 0.05f;
                Vector2 perp = Vector2.Perpendicular(direction).normalized * thickness;

                Debug.DrawLine(transform.position - (Vector3)perp, transform.position + (Vector3)direction * targetDetectionRange - (Vector3)perp, Color.magenta);
                Debug.DrawLine(transform.position + (Vector3)perp, transform.position + (Vector3)direction * targetDetectionRange + (Vector3)perp, Color.magenta);
                Debug.DrawLine(transform.position, transform.position + (Vector3)direction * targetDetectionRange, Color.magenta);

                colliders = new List<Transform>() { playerCollider.transform };
                isPlayerDetected = true;
            }
            else
            {
                colliders = null;
            }
        }
        else
        {
            colliders = null;
        }
        aiData.targets = colliders;
    }

    private IEnumerator DetectionCooldown()
    {
        canDetect = false; // Tạm khóa Detect
        yield return new WaitForSeconds(detectionCooldown);
        canDetect = true; // Cho phép Detect lại
    }

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        Gizmos.DrawWireSphere(transform.position, targetDetectionRange);

        if (colliders == null)
            return;

        Gizmos.color = Color.magenta;
        foreach (var item in colliders)
        {
            Gizmos.DrawSphere(item.position, 0.3f);
        }
    }
}
