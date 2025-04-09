using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed;
    [HideInInspector] public float ArrowDamage;
    public Vector2 arrowDirection;
    [SerializeField] Rigidbody2D rb;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }

    private void FixedUpdate()
    {
       rb.velocity = arrowDirection * arrowSpeed;
    }
}