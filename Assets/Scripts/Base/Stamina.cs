using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    public float MaxStamina;

    [SerializeField]
    private float currentStamina;

    [SerializeField]
    private float staminaRegenRate;

    [SerializeField]
    private StaminaBar staminaBar;


    private void Start()
    {
        currentStamina = MaxStamina;
    }
    public void ModifyStamia(float value)
    {
        if(currentStamina <= 0) { return;}
        currentStamina += value;
        staminaBar.SetValue(currentStamina);

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
            staminaBar.SetValue(currentStamina);
            if (currentStamina > MaxStamina)
            {
                currentStamina = MaxStamina;
            }
        }
    }


}
