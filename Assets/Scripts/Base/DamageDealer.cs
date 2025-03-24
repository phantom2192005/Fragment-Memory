using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool isSingleHit;
    [SerializeField] float damageInterval = 0.05f; // khoảng cách thời gian giưa các lần tinh hit take damage

    private float lastHitTime = 0f;

    private void DealDamage(Collider2D trigger)
    {
        if (this.gameObject.CompareTag("EnemyHitBox") && trigger.CompareTag("HurtBox"))
        {
            float currentTime = Time.time;
            if (!isSingleHit && currentTime - lastHitTime < damageInterval) return;

            lastHitTime = currentTime; // Cập nhật thời gian hit

            Debug.Log("Hit Player");
            trigger.GetComponentInParent<Health>().TakeDamage(damage);
        }
        else if (this.gameObject.CompareTag("PlayerHitBox") && trigger.CompareTag("HurtBox"))
        {
            float currentTime = Time.time;
            if (!isSingleHit && currentTime - lastHitTime < damageInterval) return;

            lastHitTime = currentTime; // Cập nhật thời gian hit

            Debug.Log("Hit Enemy");
            trigger.GetComponentInParent<Health>().TakeDamage(damage);
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
