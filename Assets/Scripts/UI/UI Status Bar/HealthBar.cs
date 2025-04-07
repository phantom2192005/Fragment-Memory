using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private RectMask2D healthFillMask;
    [SerializeField] private RectMask2D healthLostMask;

    [Header("Mask Settings")]
    [SerializeField] private float minRightMask = 97f;    // Khi máu đầy (100%)
    [SerializeField] private float maxRightMask = 326f;   // Khi máu cạn (0%)

    [Header("Effect Settings")]
    [SerializeField] private float ReduceSpeed = 0.1f; // Tốc độ làm mượt hiệu ứng mất máu

    public bool healthLostEffect;
    public float previousHealth;
    public float currentLostEffectPaddingRight;
    public float lostEffectVelocity;

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            health = player.GetComponentInChildren<Health>();
        }
        else
        {
            Debug.LogWarning("Player not found!");
            return;
        }

        previousHealth = health.maxHealth;
        currentLostEffectPaddingRight = healthLostMask.padding.z;
    }

    private void Update()
    {
        if (!healthLostEffect) return;

        float target = healthFillMask.padding.z;

        // Di chuyển giá trị hiện tại về phía target với tốc độ ReduceSpeed (giá trị càng cao -> nhanh)
        currentLostEffectPaddingRight = Mathf.MoveTowards(
            currentLostEffectPaddingRight,
            target,
            ReduceSpeed * Time.deltaTime
        );

        var padding = healthLostMask.padding;
        padding.z = currentLostEffectPaddingRight;
        healthLostMask.padding = padding;

        // Khi đã đến gần giá trị target, tắt hiệu ứng
        if (Mathf.Approximately(currentLostEffectPaddingRight, target))
        {
            healthLostEffect = false;
        }
    }
    public void SetValue(int newValue)
    {
        float healthRatio = (float)newValue / health.maxHealth;
        float rightMask = Mathf.Lerp(minRightMask, maxRightMask, 1 - healthRatio);

        // Cập nhật thanh máu chính
        var fillPadding = healthFillMask.padding;
        fillPadding.z = rightMask;
        healthFillMask.padding = fillPadding;

        if (previousHealth > newValue)
        {
            // Mất máu → bật hiệu ứng trượt
            healthLostEffect = true;
        }
        else
        {
            // Hồi máu → cập nhật ngay thanh mất máu
            var lostPadding = healthLostMask.padding;
            lostPadding.z = rightMask;
            healthLostMask.padding = lostPadding;

            // Đồng bộ giá trị cho hiệu ứng
            currentLostEffectPaddingRight = rightMask;
        }

        previousHealth = newValue;
    }

}
