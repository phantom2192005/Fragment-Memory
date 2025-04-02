using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAttackHitHandler : MonoBehaviour
{
    [Header("Knock Back")]
    public bool canKnockBack;

    public Vector2 hitDirection;
    private KnockBack knockBack;

    [Header("Hit VFX")]
    public bool canFlashHit_VFX;

    [Header("Shake Camera")]
    [SerializeField] bool canShakeCamera;
    [SerializeField] ScreenShakeProfile shakeProfile;
    private CinemachineCollisionImpulseSource impulseSource;

    [Header("HitStop")]
    public bool canHitStop;
    private HitStop HitStop;

    private void Start()
    {
        knockBack = GetComponent<KnockBack>();
        impulseSource = GetComponent<CinemachineCollisionImpulseSource>();
        HitStop = GetComponent<HitStop>();
    }
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if ((this.gameObject.CompareTag("EnemyHitBox") && trigger.tag == "HurtBox") ||
           (this.gameObject.CompareTag("PlayerHitBox") && trigger.tag == "HurtBox"))
        {
            if (canKnockBack) 
            {
                
                hitDirection = trigger.transform.position - transform.position;
                knockBack.SetKnockBack(trigger.GetComponentInParent<Rigidbody2D>());
                knockBack.CallKnockback(hitDirection,Vector2.zero,Vector2.zero);

            }
            if (canFlashHit_VFX)
            {

            }

            if (canShakeCamera)
            {
                if (CameraShakeManager.instance.canShake)
                {
                    CameraShakeManager.instance.ScreenShakeFromProfile(shakeProfile,impulseSource);
                }
            }

            if(canHitStop)
            {
                HitStop.Stop();
            }

        }
           
    }

    private void OnTriggerStay2D(Collider2D trigger)
    {
        if ((this.gameObject.CompareTag("EnemyHitBox") && trigger.tag == "HurtBox") ||
           (this.gameObject.CompareTag("PlayerHitBox") && trigger.tag == "HurtBox"))
        {

        }
    }
}
