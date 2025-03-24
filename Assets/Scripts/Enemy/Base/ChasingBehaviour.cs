using UnityEngine;

public class ChasingBehavior : MonoBehaviour
{
    [Header("Chasing Settings")]
    public float chasingSpeed;
    public Transform rayCastOrigin;
    public float rayCastLength;
    public LayerMask targetLayer;

    private BaseEnemy baseEnemy;
    private GameObject target;
    private RaycastHit2D hitInfo;
    private Vector2 direction;

    void Start()
    {
        baseEnemy = GetComponent<BaseEnemy>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        PerformRaycast();
        DrawRaycast();
    }

    private void PerformRaycast()
    {
        if (target == null) return;

        direction = (target.transform.position - rayCastOrigin.position).normalized;
        hitInfo = Physics2D.Raycast(rayCastOrigin.position, direction, rayCastLength, targetLayer);

        if (hitInfo.collider != null && hitInfo.collider.tag == "Player" && baseEnemy.IsAttacking == false)
        {
            baseEnemy.IsChasing = true;
            baseEnemy.IsPatrolling = false;
        }
        else
        {
            baseEnemy.IsChasing = false;
            baseEnemy.IsPatrolling = true;
        }
    }

    private void DrawRaycast()
    {
        bool hitPlayer = hitInfo.collider != null && hitInfo.collider.CompareTag("Player");
        Color rayColor = hitPlayer ? Color.green : Color.red;
        Vector3 endPoint = (hitInfo.collider != null) ? hitInfo.point : rayCastOrigin.position + (Vector3)direction * rayCastLength;
        Debug.DrawLine(rayCastOrigin.position, endPoint, rayColor);
    }


    public void ChasePlayer()
    {
        if (baseEnemy.haveRun)
        {
            baseEnemy.animator.SetBool("IsRun", true);
        }
        baseEnemy.FlipObject(target.transform);
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, chasingSpeed * Time.deltaTime);
    }
}
