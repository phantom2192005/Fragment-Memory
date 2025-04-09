using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField]
    private Stamina stamina;

    [SerializeField]
    private RectTransform barRect;

    [SerializeField]
    private RectMask2D mask;

    private float initialRightMask;

    [SerializeField]
    private float minRightMask = 165f;   // Khi đầy (100%)

    [SerializeField]
    private float maxRightMask = 326f;  // Khi cạn (0%)

    private void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            stamina = player.GetComponentInChildren<Stamina>();
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }

    public void SetValue(float newValue)
    {
        float staminaRatio = newValue / stamina.MaxStamina;
        float newRightMask = Mathf.Lerp(minRightMask, maxRightMask, 1 - staminaRatio);

        var padding = mask.padding;
        padding.z = newRightMask;
        mask.padding = padding;
    }
}
