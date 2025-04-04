using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField] private float patrolSpeed = 2f; // Set a default speed
    [SerializeField] private float waitTime = 2f; // Set a default wait time
    private float waitTimer;
    private Transform[] moveSpots;
    private int randomSpot;
    public GameObject Path;
    private EnemeyController baseEnemy;

    void Start()
    {
        baseEnemy = GetComponent<EnemeyController>();
        waitTimer = waitTime;

        if (Path != null)
        {
            moveSpots = new Transform[Path.transform.childCount];
            for (int i = 0; i < Path.transform.childCount; i++)
            {
                moveSpots[i] = Path.transform.GetChild(i);
            }
        }

        if (moveSpots.Length > 0)
        {
            randomSpot = Random.Range(0, moveSpots.Length);
            baseEnemy.FlipObject(moveSpots[randomSpot].position);
        }
    }

    public void Patrol()
    {
        if (moveSpots.Length == 0) return;

        baseEnemy.isIdling = false;

        // Calculate the direction to the current random spot
        Vector2 direction = (moveSpots[randomSpot].position - transform.position).normalized;

        // Move the enemy by updating its position
        transform.position += (Vector3)direction * patrolSpeed * Time.deltaTime;

        // Check if the enemy needs to flip
        if (direction.x > 0)
        {
            baseEnemy.FlipObject(moveSpots[randomSpot].position); // Flip to face right
        }
        else if (direction.x < 0)
        {
            baseEnemy.FlipObject(moveSpots[randomSpot].position); // Flip to face left
        }

        // Check if the enemy has reached the patrol point
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTimer <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTimer = waitTime;
                baseEnemy.FlipObject(moveSpots[randomSpot].position); // Flip to face the new target
            }
            else
            {
                if (baseEnemy.haveRun)
                {
                    baseEnemy.animator.SetBool("IsRun", false);
                }
                baseEnemy.isIdling = true;
                waitTimer -= Time.deltaTime; // Decrease the wait timer
            }
        }
        else
        {
            if (baseEnemy.haveRun)
            {
                baseEnemy.animator.SetBool("IsRun", true);
            }
        }
    }
}