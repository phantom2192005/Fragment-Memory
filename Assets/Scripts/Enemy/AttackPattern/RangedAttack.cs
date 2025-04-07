using System.Collections;
using UnityEngine;

public class RangedAttack : MonoBehaviour, IAttackPattern
{
    [SerializeField] private float delayTimeFire = 0.0f;
    [SerializeField] private float coolDownTime = 3f;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private Vector2 OffSetDistance;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private string[] rangedAttackAnimationParameters;

    private Vector3 spawnPoint;
    [SerializeField] Vector3 cachedtargetPoint;
    private Transform target;
    private EnemeyController enemyController;
    private float cooldownTimer;

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
        StartCoroutine(DelayedFire());
    }

    private IEnumerator DelayedFire()
    {
        spawnPoint = projectilePrefab.GetComponent<BaseProjectile>()
                    .CalculateSpawnPoint(Camera.main, enemyController.transform.position, target.position);
        cachedtargetPoint = target.position;
        yield return new WaitForSeconds(delayTimeFire);

        GameObject projectile = Instantiate(projectilePrefab, spawnPoint + (Vector3)OffSetDistance, Quaternion.identity);
        projectile.GetComponent<BaseProjectile>().hitPosition = cachedtargetPoint;
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
