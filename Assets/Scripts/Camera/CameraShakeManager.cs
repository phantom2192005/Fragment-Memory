using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    [SerializeField] private float globalShakeForce = 1.0f;
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

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }

    public void ScreenShakeFromProfile(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        //apply settings
        SetUpScreenShakeSettings(profile, impulseSource);

        //sreen shake
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
        
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
