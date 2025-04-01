using System.Collections.Generic;
using UnityEngine;

public class ChasingBehavior : MonoBehaviour
{
    [Header("Chasing Settings")]
    public float chasingSpeed;
    public float retreatSpeed;
    public float retreatDistance;
    public float maxChaseDistance;

    private BaseEnemy baseEnemy;
    private Transform[] moveSpots;
    private int randomSpot;

    public GameObject PatrolPath;
    private TargetDetector targetDetector;
    [SerializeField]
    private EnemyMovementAI enemyMovementAI;

    public Vector2 direction;
    private void Start()
    {
        baseEnemy = GetComponent<BaseEnemy>();
        targetDetector = GetComponentInChildren<TargetDetector>();

        if (PatrolPath != null)
        {
            moveSpots = new Transform[PatrolPath.transform.childCount];
            for (int i = 0; i < PatrolPath.transform.childCount; i++)
            {
                moveSpots[i] = PatrolPath.transform.GetChild(i);
            }
        }
    }

    void Update()
    {
        if (targetDetector.isDetected)
        {
            baseEnemy.IsPlayerDetected = true;
            baseEnemy.IsPatrolling = false;
        }
    }

    public void ChasePlayer()
    {
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

        // Nếu mục tiêu quá xa, quay lại tuần tra
        if (distanceToTarget >= maxChaseDistance || enemyMovementAI.contextData.currentTarget == null)
        {
            targetDetector.isDetected = false;
            baseEnemy.IsPlayerDetected = false;
            baseEnemy.isAttacking = false;
            baseEnemy.IsPatrolling = true;
            return;
        }

         // Lấy hướng di chuyển
        direction = enemyMovementAI.direction;

        // Di chuyển bằng transform.position
        transform.position += (Vector3)(direction * chasingSpeed * Time.deltaTime);

        if (baseEnemy.haveRun)
        {
            baseEnemy.animator.SetBool("IsRun", true);
        }
    }

}
