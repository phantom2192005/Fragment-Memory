using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health _health;

    [SerializeField]
    private RectTransform _barRect;

    [SerializeField]
    private RectMask2D _mask;

    private float _maxRightMask;
    private float _initialRightMask;

    private void Start()
    {
        //  x = left, w = top, y = bottom , z = right
        _maxRightMask = _barRect.rect.width  - _mask.padding.x - _mask.padding.z;
        _initialRightMask = _mask.padding.z;
    }
    public void SetValue(int newValue)
    {
        float healthRatio = (float)newValue / _health.maxHealth; // Tỷ lệ máu còn lại
        float newRightMask = Mathf.Lerp(97, 335, 1 - healthRatio); // Nội suy giữa 97 và 335

        var padding = _mask.padding;
        padding.z = newRightMask;
        _mask.padding = padding;
    }


}