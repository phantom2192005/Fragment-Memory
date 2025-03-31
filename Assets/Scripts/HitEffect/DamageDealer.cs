using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool isSingleHit;
    [SerializeField] float damageInterval = 0.05f; // khoảng cách thời gian giữa các lần tính hit
    [SerializeField] bool canKnockBack;
    [SerializeField] bool canShakeCamera;
    [SerializeField] ScreenShakeProfile shakeProfile;
    Health targetHealth;
    HitStop hitStop;
    private CinemachineCollisionImpulseSource impulseSource;



    private float lastHitTime = 0f;

    private void Start()
    {
        hitStop = GetComponent<HitStop>();
        impulseSource = GetComponent<CinemachineCollisionImpulseSource>();
    }

    private void DealDamage(Collider2D trigger)
    {
        Vector2 hitDirection = (trigger.transform.position - transform.position).normalized;
        //Debug.Log(hitDirection);

        if ((this.gameObject.CompareTag("EnemyHitBox") && trigger.tag == "HurtBox") ||
            (this.gameObject.CompareTag("PlayerHitBox") && trigger.tag == "HurtBox"))
        {
            // Time.time return how long game have been run
            float elapsedTime = Time.time - lastHitTime; // Thời gian trôi qua kể từ lần hit trước đó
            if (!isSingleHit && elapsedTime < damageInterval) return;

            lastHitTime = Time.time; // Cập nhật thời gian hit

            Debug.Log(this.gameObject.CompareTag("EnemyHitBox") ? "Hit Player" : "Hit Enemy");

            targetHealth = trigger.GetComponent<Health>();
            targetHealth.canKnockBack = canKnockBack;
            targetHealth.TakeDamage(damage, hitDirection);

            if (impulseSource != null && canShakeCamera && shakeProfile != null)
            {
                //CameraShakeManager.instance.CameraShake(impulseSource);
                CameraShakeManager.instance.ScreenShakeFromProfile(shakeProfile, impulseSource);
            }
            if (hitStop != null)
            {
                hitStop.Stop();
            }
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
