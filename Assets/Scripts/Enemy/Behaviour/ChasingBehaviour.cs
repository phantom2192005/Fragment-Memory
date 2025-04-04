using UnityEngine;

public class ChasingBehavior : MonoBehaviour
{
    [Header("Chasing Settings")]
    public float chasingSpeed;

    [Header("References")]
    [SerializeField] private EnemyMovementAI enemyMovementAI;

    private EnemeyController baseEnemy;
    private TargetDetector targetDetector;
    private GameObject player;

    private void Start()
    {
        baseEnemy = GetComponent<EnemeyController>();
        targetDetector = GetComponentInChildren<TargetDetector>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
    }

    public void ChasePlayer()
    {
        baseEnemy.isPatrolling = false;
        if (baseEnemy.haveRun) { baseEnemy.animator.SetBool("IsRun", true); }
        if (player == null) return;

        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Debug.Log("Chase");
        Vector2 direction = enemyMovementAI.direction;
        transform.position += (Vector3)(direction * chasingSpeed * Time.deltaTime);
    }
}