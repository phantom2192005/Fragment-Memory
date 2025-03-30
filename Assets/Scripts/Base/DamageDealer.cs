using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool isSingleHit;
    [SerializeField] float damageInterval = 0.05f; // khoảng cách thời gian giưa các lần tinh hit take damage
    [SerializeField] bool canKnockBack;

    private float lastHitTime = 0f;

    private void DealDamage(Collider2D trigger)
    {
        Vector2 hitDirection = (trigger.transform.position - transform.position).normalized;

        Debug.Log(hitDirection);

        if (this.gameObject.CompareTag("EnemyHitBox") && trigger.tag == "HurtBox")
        {
            float currentTime = Time.time;
            if (!isSingleHit && currentTime - lastHitTime < damageInterval) return;

            lastHitTime = currentTime; // Cập nhật thời gian hit

            Debug.Log("Hit Player");

            Health targetHealth = trigger.GetComponent<Health>();
            targetHealth.canKnockBack = canKnockBack;
            targetHealth.TakeDamage(damage, hitDirection);
            
        }
        else if (this.gameObject.CompareTag("PlayerHitBox") && trigger.tag == "HurtBox")
        {
            float currentTime = Time.time;
            if (!isSingleHit && currentTime - lastHitTime < damageInterval) return;

            lastHitTime = currentTime; // Cập nhật thời gian hit

            Debug.Log("Hit Enemy");

            Health targetHealth = trigger.GetComponent<Health>();
            targetHealth.canKnockBack = canKnockBack;
            targetHealth.TakeDamage(damage, hitDirection);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (isSingleHit) DealDamage(trigger);
    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if (!isSingleHit) DealDamage(trigger);
    }
}
