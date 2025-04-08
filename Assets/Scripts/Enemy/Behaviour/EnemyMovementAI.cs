using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMovementAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    public ContextData contextData;

    [SerializeField]
    private ContextSolver contextSolver;

    [SerializeField]
    private float detectionDelay = 0.05f;



    [SerializeField]
    public Vector2 direction;

    private void Start()
    {
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(contextData);
        }

        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach (SteeringBehaviour behaviour in steeringBehaviours)
        {
            (danger, interest) = behaviour.GetSteering(danger, interest, contextData);
        }
        direction = contextSolver.GetDirectionToMove(steeringBehaviours, contextData);
    }
}