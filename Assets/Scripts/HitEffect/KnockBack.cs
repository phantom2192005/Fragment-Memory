using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KnockBack : MonoBehaviour
{
    public float knockBackTime = 0.2f;
    public float hitDirectionForce = 10f;
    public float constForce = 5f;
    public float inputForce = 7.5f;

    private Rigidbody2D rb;

    private Coroutine knockbackCoroutine;

    public bool IsBeingKnockedBack {  get; private set; }

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }
    public IEnumerator KnockBackAction(Vector2 hitDirection, Vector2 constantForceDirection, Vector2 inputDirection)
    {
        IsBeingKnockedBack = true;

        Vector2 hitForce;
        Vector2 constantForce;
        Vector2 knockbackForce;
        Vector2 combineForce;

        hitForce = hitDirection * hitDirectionForce;
        constantForce =  constantForceDirection * constForce;

            
        float _elapsedTime = 0f;
        while (_elapsedTime < knockBackTime) 
        {
            // irerate the timer
            _elapsedTime += Time.fixedDeltaTime;

            //combine hitForce and constantForce
            knockbackForce = hitForce + constantForce;

            if (inputDirection != Vector2.zero) 
            {
                combineForce = knockbackForce + (inputDirection * inputForce);
            }
            else
            {
                combineForce = knockbackForce;
            }
             rb.velocity = combineForce;

            yield return new WaitForFixedUpdate();
        }
        IsBeingKnockedBack = false;
        rb.velocity = Vector2.zero;
    }

    public void CallKnockback(Vector2 hitDirection, Vector2 constantForceDirection, Vector2 inputDirection)
    {
        knockbackCoroutine = StartCoroutine(KnockBackAction(hitDirection, constantForceDirection, inputDirection));
    }
}
