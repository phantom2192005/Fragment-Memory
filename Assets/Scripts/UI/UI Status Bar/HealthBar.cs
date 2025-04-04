using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private RectTransform barRect;

    [SerializeField]
    private RectMask2D mask;

    private float initialRightMask;

    [SerializeField]
    private float minRightMask = 97f;   // Khi đầy (100%)

    [SerializeField]
    private float maxRightMask = 326f;  // Khi cạn (0%)

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
        }
    }

    public void SetValue(int newValue)
    {
        float healthRatio = (float)newValue / health.maxHealth;
        float newRightMask = Mathf.Lerp(minRightMask, maxRightMask, 1 - healthRatio);

        var padding = mask.padding;
        padding.z = newRightMask;
        mask.padding = padding;
    }
}
