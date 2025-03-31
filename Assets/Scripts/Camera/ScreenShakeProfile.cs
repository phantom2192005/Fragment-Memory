using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="ScreenShake/New Profile")]
public class ScreenShakeProfile : ScriptableObject
{
    [Header("Impulse Source Settings")]
    public float impactTime = 0.2f;
    public float impactForce = 1.0f;
    public Vector3 defaultVelocity = new Vector3(0f, -1f, 0f);
    public AnimationCurve impulseCurve;

    [Header("Impulse Listener Setting")]
    public float listenerAmplitude = 1.0f;
    public float listenerFrequecy = 1.0f;
    public float listenerDuration = 1.0f;
}