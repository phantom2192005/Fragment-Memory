using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool isSingleHit;
    [SerializeField] float damageInterval = 0.05f; // khoảng cách thời gian giữa các lần tính hit
    Health targetHealth;

    private float lastHitTime = 0f;

    private void DealDamage(Collider2D trigger)
    {
        Vector2 hitDirection = (trigger.transform.position - transform.position).normalized;

        if ((this.gameObject.CompareTag("EnemyHitBox") && trigger.tag == "HurtBox") ||
            (this.gameObject.CompareTag("PlayerHitBox") && trigger.tag == "HurtBox"))
        {
            Debug.Log(this.gameObject.CompareTag("EnemyHitBox") ? "Hit Player" : "Hit Enemy");

            
            float elapsedTime = Time.time - lastHitTime;
            if (!isSingleHit && elapsedTime < damageInterval) return;

            lastHitTime = Time.time;

            targetHealth = trigger.GetComponent<Health>();
            targetHealth.TakeDamage(damage);
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
