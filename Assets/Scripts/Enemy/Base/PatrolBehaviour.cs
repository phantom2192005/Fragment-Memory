using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float waitTime;
    public float waitTimer;
    private Transform[] moveSpots;
    private int randomSpot;
    public GameObject Path;
    private BaseEnemy baseEnemy;

    void Start()
    {
        baseEnemy = GetComponent<BaseEnemy>();
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
            baseEnemy.FlipObject(moveSpots[randomSpot]);
            
        }
    }

    public void Patrol()
    {
        if (moveSpots.Length == 0) return;

        baseEnemy.IsIdling = false; 
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, patrolSpeed * Time.deltaTime);
        if (baseEnemy.haveRun)
        {
            baseEnemy.animator.SetBool("IsRun", true);
        }
        if (transform.position.x > moveSpots[randomSpot].position.x) // handle flip after out chasing
        {
            baseEnemy.FlipObject(moveSpots[randomSpot]);
        }
       
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTimer <= 0)
            {
               
                randomSpot = Random.Range(0, moveSpots.Length); 
                waitTimer = waitTime;
                baseEnemy.FlipObject(moveSpots[randomSpot]);
            }
            else
            {
                if (baseEnemy.haveRun)
                {
                    baseEnemy.animator.SetBool("IsRun", false);
                }
                baseEnemy.IsIdling = true;
                waitTimer -= Time.deltaTime;
            }
        }
    }
}
