using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    public Vector3 hitPosition;
    public Vector2 bulletVelocity;

    [Header("Shadow Settings")]
    [SerializeField] protected GameObject shadowPrefab;
    [SerializeField] protected AnimationCurve scaleCurve;
    [SerializeField] protected AnimationCurve alphaCurve;

    protected GameObject shadowInstance;
    protected Transform shadowTransform;
    protected SpriteRenderer shadowRenderer;

    protected Vector3 cachedScale = Vector3.one;
    protected float cachedAlpha = 1f;

    protected float startingDistance;
    protected float currentDistance;
    protected float ratioOfChange;

    protected bool hasInitializedDistance = false;

    protected Camera mainCamera;
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        // Auto-set hit position to player if not already set
        if (hitPosition == Vector3.zero)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                hitPosition = player.transform.position;
            }
        }
    }

    protected virtual void Update()
    {
        InitializeDistanceOnce();

        Move();
        Hit();
        UpdateShadow();
    }

    protected virtual void Move()
    {
        transform.position += (Vector3)(bulletVelocity * Time.deltaTime);
    }

    protected virtual void Hit()
    {
        if (Vector2.Distance(transform.position, hitPosition) < 0.5f)
        {
            animator?.SetBool("IsHit", true);
            bulletVelocity = Vector2.zero;
            transform.position = hitPosition;
        }
    }

    protected void InitializeDistanceOnce()
    {
        if (!hasInitializedDistance)
        {
            startingDistance = Vector2.Distance(transform.position, hitPosition);
            hasInitializedDistance = true;
        }

        currentDistance = Vector2.Distance(transform.position, hitPosition);
        ratioOfChange = 1f - (currentDistance / startingDistance);
    }

    protected virtual void UpdateShadow()
    {
        if (shadowPrefab == null) return;

        if (shadowInstance == null)
            PrepareShadow();

        if (shadowTransform != null)
        {
            // Update scale
            float scale = scaleCurve.Evaluate(ratioOfChange);
            cachedScale.Set(scale, scale, 1f);
            shadowTransform.localScale = cachedScale;

            // Update alpha
            if (shadowRenderer != null)
            {
                cachedAlpha = alphaCurve.Evaluate(ratioOfChange);
                Color color = shadowRenderer.color;
                color.a = cachedAlpha;
                shadowRenderer.color = color;
            }
        }

        if (currentDistance <= 0.01f && shadowInstance != null)
        {
            Destroy(shadowInstance);
            shadowInstance = null;
            shadowTransform = null;
            shadowRenderer = null;
        }
    }

    protected virtual void PrepareShadow()
    {
        shadowInstance = Instantiate(shadowPrefab, hitPosition, shadowPrefab.transform.rotation);
        shadowTransform = shadowInstance.transform;
        shadowRenderer = shadowInstance.GetComponent<SpriteRenderer>();
    }

    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    public virtual Vector3 CalculateSpawnPoint(Camera mainCamera, Vector3 firingPointPosition, Vector3 targetPosition)
    {
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float x_spawn = targetPosition.x;
        float y_spawn = mainCamera.transform.position.y + (cameraHeight / 2f);
        return new Vector3(x_spawn, y_spawn, 0f);
    }
}
