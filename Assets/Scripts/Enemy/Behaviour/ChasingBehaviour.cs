using System.Collections;
using UnityEngine;

public class ChasingBehavior : MonoBehaviour
{
    [Header("Chasing Settings")]
    public float chasingSpeed;
    public float minRetreatSpeed;
    public float maxRetreatSpeed;
    public float maxChaseDistance;

    [Header("Retreat Timing")]
    public float minRetreatDelay = 1f;
    public float maxRetreatDelay = 3f;

    [Header("References")]
    [SerializeField] private Transform PatrolSpot;
    [SerializeField] private Collider2D retreatAreaCollider;
    [SerializeField] private EnemyMovementAI enemyMovementAI;

    private BaseEnemy baseEnemy;
    private TargetDetector targetDetector;
    private GameObject player;
    private Vector2 direction;
    private float currentRetreatSpeed;
    private bool isRetreating;

    private void Start()
    {
        baseEnemy = GetComponent<BaseEnemy>();
        targetDetector = GetComponentInChildren<TargetDetector>();
        currentRetreatSpeed = minRetreatSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (targetDetector.isPlayerDetected)
        {
            baseEnemy.IsPlayerDetected = true;
        }

        if (isRetreating)
        {
            currentRetreatSpeed = Mathf.Clamp(currentRetreatSpeed + Time.deltaTime, minRetreatSpeed, maxRetreatSpeed);
        }
    }

    public void ChasePlayer()
    {
        if (player == null) return;

        float distanceToReturn = Vector2.Distance(transform.position, PatrolSpot.position);

        // Quay lại vị trí tuần tra nếu quá xa
        if (distanceToReturn >= maxChaseDistance || enemyMovementAI.contextData.currentTarget == null)
        {
            ResetChaseState();
            return;
        }

        // Kiểm tra trong vùng retreat (dựa trên collider thay vì khoảng cách)
        bool shouldRetreat = retreatAreaCollider != null && retreatAreaCollider.OverlapPoint(player.transform.position);

        if (shouldRetreat)
        {
            StartRetreating();
        }
        else
        {
            StopRetreating();
        }

        float speed = isRetreating ? currentRetreatSpeed : chasingSpeed;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (baseEnemy.haveRun)
        {
            baseEnemy.animator.SetBool("IsRun", true);
        }
    }

    private void StartRetreating()
    {
        direction = -enemyMovementAI.direction;
        isRetreating = true;
    }

    private void StopRetreating()
    {
        direction = enemyMovementAI.direction;
        isRetreating = false;
        currentRetreatSpeed = minRetreatSpeed;
    }

    private void ResetChaseState()
    {
        targetDetector.isPlayerDetected = false;
        baseEnemy.IsPlayerDetected = false;
        baseEnemy.isAttacking = false;
        baseEnemy.IsPatrolling = true;
        StopRetreating();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartRetreating();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopRetreating();
        }
    }
}
