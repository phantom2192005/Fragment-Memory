using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public Vector3 hitPosition;
    public Vector2 bulletVelocity;
    protected Transform targetTransform;
    protected Camera mainCamera;
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            targetTransform = player.transform;
            hitPosition = targetTransform.position;
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }

    protected virtual void Update()
    {
        Move();
        Hit();
    }

    protected virtual void Move()
    {
        transform.position += (Vector3)(bulletVelocity * Time.deltaTime);
    }

    protected virtual void Hit()
    {
        if (Vector2.Distance(transform.position, hitPosition) < 0.5f)
        {
            animator.SetBool("IsHit", true);
            bulletVelocity = Vector2.zero;
            transform.position = hitPosition;
        }
    }

    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    public virtual Vector3 CalculateSpawnPoint(Camera mainCamera,Transform firingPoint, Transform targetTransform)
    {
       return Vector2.zero;
    }
}
