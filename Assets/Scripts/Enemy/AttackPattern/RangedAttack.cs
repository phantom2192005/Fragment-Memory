using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackPattern
{
    [SerializeField] private float coolDownTime = 3f;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private string[] rangedAttackAnimationParameters; // Danh sách animation

    private Vector3 firePoint;
    private Transform target;
    private EnemeyController enemyController;

    public float cooldownTimer;

    void Start()
    {
        cooldownTimer = 0;
        enemyController = GetComponentInParent<EnemeyController>();
        target = FindFirstObjectByType<PlayerController>().transform;
    }

    public bool CanAttack()
    {
        return cooldownTimer <= 0 && enemyController.targetDetecter.isPlayerDetected;
    }

    public void ExecuteAttack()
    {
        if (CanAttack())
        {
            cooldownTimer = coolDownTime;

            if (rangedAttackAnimationParameters.Length > 0)
            {
                int index = Random.Range(0, rangedAttackAnimationParameters.Length);
                string selectedAnim = rangedAttackAnimationParameters[index];

                Debug.Log($"Execute Ranged Attack: {selectedAnim}");
                enemyController.SetAnimatorBoolParameter(selectedAnim, true);
            }
        }
    }

    public void FireProjectile()
    {
        if (target == null) return;

        firePoint = projectilePrefab.GetComponent<BaseProjectile>().CalculateSpawnPoint(Camera.main, enemyController.transform, target.transform);
        GameObject projectile = Instantiate(projectilePrefab, firePoint, Quaternion.identity);
        projectile.GetComponent<BaseProjectile>().bulletVelocity = velocity;
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
