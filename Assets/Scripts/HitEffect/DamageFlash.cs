using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;
    float currentFlashAmount = 0f;
    float elapsedTime = 0f;

    private SpriteRenderer _spriteRenderer;
    private Material _material;
    private Coroutine _damageFlashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();

        if (_spriteRenderer == GetComponent<SpriteRenderer>())
        {
            _spriteRenderer = transform.parent?.GetComponentInParent<SpriteRenderer>();
        }
        Init();
    }

    private void Init()
    {
        _material = new Material(_spriteRenderer.material);
        _spriteRenderer.material = _material;
    }

    public void CallDamageFlash()
    {
        if (_damageFlashCoroutine != null)
        {
            StopCoroutine(_damageFlashCoroutine);
        }

        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }


    private IEnumerator DamageFlasher()
    {
        SetFlashColor();
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1.0f, 0.0f, elapsedTime / _flashTime);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
        currentFlashAmount = 0f;
        elapsedTime = 0f;
        SetFlashAmount(0f);
    }

    private void SetFlashColor()
    {
        _material.SetColor("_FlashColor", _flashColor);
    }

    private void SetFlashAmount(float amount)
    {
        _material.SetFloat("_FlashAmount", amount);
    }
}
