using System.Reflection.Emit;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackPattern
{
    [SerializeField] private float coolDownTime = 3f;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private GameObject projectilePrefab;
    private Vector3 firePoint;
    [SerializeField] private Transform target;
    EnemeyController enemyController;

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
                enemyController.SetAnimatorBoolParameter("IsRangedAttack", true);
        }
    }

    public void FireProjectile()
    {
        firePoint = BaseProjectile.CalculateSpawnPoint(Camera.main, target.transform);
        projectilePrefab.GetComponent<BaseProjectile>().bulletVelocity = velocity;
        Instantiate(projectilePrefab, firePoint, Quaternion.identity);
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
