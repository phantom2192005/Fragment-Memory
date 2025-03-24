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

    public static Vector3 CalculateSpawnPoint(Camera mainCamera, Transform targetTransform)
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found!");
                return Vector3.zero;
            }
        }

        if (targetTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                targetTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found!");
                return Vector3.zero;
            }
        }

        float cameraHeight = mainCamera.orthographicSize * 2;
        float x_spawn = targetTransform.position.x;
        float y_spawn = mainCamera.transform.position.y + (cameraHeight / 2);

        return new Vector3(x_spawn, y_spawn, 0);
    }
}
