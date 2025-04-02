using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;
    public bool canShake = true;

    [SerializeField] CinemachineImpulseListener impulseListener;
    private CinemachineImpulseDefinition impulseDefinition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private IEnumerator canShakeDelay()
    {
        canShake = false;
        yield return new WaitForSeconds(0.1f);
        canShake = true;
    }

    public void ScreenShakeFromProfile(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        Debug.Log("camera shake Profile");
        //apply settings
        SetUpScreenShakeSettings(profile, impulseSource);

        //sreen shake
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
        StartCoroutine(canShakeDelay());

    }

    private void SetUpScreenShakeSettings(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        // change the impulse source settings
        impulseDefinition.m_ImpulseDuration = profile.listenerDuration;
        impulseSource.m_DefaultVelocity = profile.defaultVelocity;
        impulseDefinition.m_CustomImpulseShape = profile.impulseCurve;

        //change the impulse listener settings
        impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequecy;
        impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;
    }
}
