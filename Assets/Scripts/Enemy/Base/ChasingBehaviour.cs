using System.IO;
using UnityEngine;

public class ChasingBehavior : MonoBehaviour
{
    [Header("Chasing Settings")]
    public float chasingSpeed;
    public float retreatSpeed;

    public Transform rayCastOrigin;
    public float rayCastLength;
    public LayerMask targetLayer;

    private BaseEnemy baseEnemy;
    private GameObject target;
    public float retreatDistance;
    public bool isRetreating;
    

    public GameObject PatrolPath;
    private Transform[] moveSpots;
    private int randomSpot;
    public float maxChaseDistance;

    private RaycastHit2D hitInfo;
    private Vector2 direction;
    

    void Start()
    {
        baseEnemy = GetComponent<BaseEnemy>();
        target = GameObject.FindGameObjectWithTag("Player");

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
        if(baseEnemy.isPlayerDetected == false)
        {
            PerformRaycast(); 
            DrawRaycast();
        }
       
    }

    private void PerformRaycast()
    {
        if (target == null) return;

        direction = (target.transform.position - rayCastOrigin.position).normalized;
        hitInfo = Physics2D.Raycast(rayCastOrigin.position, direction, rayCastLength, targetLayer);

        if (hitInfo.collider != null && hitInfo.collider.tag == "Player")
        {
            baseEnemy.IsPlayerDetected = true;
            baseEnemy.IsPatrolling = false;
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
        randomSpot = Random.Range(0, moveSpots.Length);
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].transform.position) >= maxChaseDistance)
        {
            baseEnemy.IsPlayerDetected = false;
            baseEnemy.IsPatrolling = true;
        }
        if (baseEnemy.haveRun)
        {
            baseEnemy.animator.SetBool("IsRun", true);
        }
        baseEnemy.FlipObject(target.transform);
        if (Vector2.Distance(transform.position,target.transform.position) < retreatDistance) 
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, -retreatSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, chasingSpeed * Time.deltaTime);
        }
        
    }
}
