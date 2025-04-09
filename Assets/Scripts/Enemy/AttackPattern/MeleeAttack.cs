using UnityEngine;

public class MeleeAttack : MonoBehaviour, IAttackPattern
{
    [SerializeField] private float coolDownTime = 2f;
    [SerializeField] private string[] meleeAttackAnimationParameters; // Danh sách tên tham số animator

    public float cooldownTimer;
    private EnemeyController enemyController;

    void Start()
    {
        cooldownTimer = 0;
        enemyController = GetComponentInParent<EnemeyController>();
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

            // Chọn random 1 animation
            if (meleeAttackAnimationParameters.Length > 0)
            {
                int index = Random.Range(0, meleeAttackAnimationParameters.Length);
                string selectedAnim = meleeAttackAnimationParameters[index];

                Debug.Log($"Execute Melee Attack: {selectedAnim}");
                enemyController.SetAnimatorBoolParameter(selectedAnim, true);
            }
        }
    }

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
