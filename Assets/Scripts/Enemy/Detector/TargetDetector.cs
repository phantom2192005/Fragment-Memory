using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : Detector
{
    [SerializeField]
    private float targetDetectionRange = 5;

    [SerializeField]
    private LayerMask obstaclesLayerMask, playerLayerMask;

    public bool isDetected;

    [SerializeField]
    private bool showGizmos = false;

    //gizmo parameters
    private List<Transform> colliders;

    public override void Detect(ContextData aiData)
    {
        //Find out if player is near
        Collider2D playerCollider =
            Physics2D.OverlapCircle(transform.position, targetDetectionRange, playerLayerMask);

        if (playerCollider != null)
        {
            //Check if you see the player
            Vector2 direction = (playerCollider.transform.position - transform.position).normalized;
            RaycastHit2D hit =
                Physics2D.Raycast(transform.position, direction, targetDetectionRange, obstaclesLayerMask);

            //Make sure that the collider we see is on the "Player" layer
            if (hit.collider != null && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
            {
                // Vẽ nhiều đường song song để làm dày tia
                float thickness = 0.1f; // Điều chỉnh độ dày
                Vector2 perp = Vector2.Perpendicular(direction).normalized * thickness;

                Debug.DrawLine(transform.position - (Vector3)perp, transform.position + (Vector3)direction * targetDetectionRange - (Vector3)perp, Color.magenta);
                Debug.DrawLine(transform.position + (Vector3)perp, transform.position + (Vector3)direction * targetDetectionRange + (Vector3)perp, Color.magenta);
                Debug.DrawLine(transform.position, transform.position + (Vector3)direction * targetDetectionRange, Color.magenta); // Đường trung tâm

                colliders = new List<Transform>() { playerCollider.transform };
                isDetected = true;
            }

            else
            {
                colliders = null;
            }
        }
        else
        {
            //Enemy doesn't see the player
            colliders = null;
        }
        aiData.targets = colliders;
    }

    private void OnDrawGizmosSelected()
    {
        if (showGizmos == false)
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