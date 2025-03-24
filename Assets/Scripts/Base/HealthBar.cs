using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Gradient gradient;
    public Slider fill_slider;
    public Image fill;
    public Transform target;
    public Vector2 Offset;
    public void SetMaxHealth(int value)
    {
        fill_slider.maxValue = value;
        fill_slider.value = value;

        fill.color = gradient.Evaluate(1f);

    }
    private void LateUpdate()
    {
        if (transform.parent == null || transform.parent.parent == null) return;

        Vector3 grandParentScale = transform.parent.parent.localScale;
        Vector3 localScale = transform.localScale;

        localScale.x = Mathf.Abs(localScale.x) * Mathf.Sign(grandParentScale.x);
        transform.localScale = localScale;
    }

    private void Update()
    {
        if(target == null) return;
        transform.position = target.position + (Vector3)Offset;
    }


    public void SetHealth(int value)
    {
        fill_slider.value = Mathf.Clamp(value, 0, fill_slider.maxValue);

        fill.color = gradient.Evaluate(fill_slider.normalizedValue);
        if (fill_slider.value == 0) 
        {
            Destroy(gameObject);
        }
    }
}
