using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    private float MaxStamina;

    [SerializeField]
    private float currentStamina;

    [SerializeField]
    private float staminaRegenRate;


    private void Start()
    {
        currentStamina = MaxStamina;
    }
    public void ModifyStamia(float value)
    {
        if(currentStamina <= 0) { return;}
        currentStamina += value;

    }
    public float GetCurrentStamia()
    {
        return currentStamina;
    }

    public void Update()
    {
        if (currentStamina < MaxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > MaxStamina)
            {
                currentStamina = MaxStamina;
            }
        }
    }


}
