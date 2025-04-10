using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    [SerializeField]
    public float maxStamina;

    [SerializeField]
    private float currentStamina;

    [SerializeField]
    private float staminaRegenRate;

    [SerializeField]
    private StaminaBar staminaBar;


    private void Start()
    {
        currentStamina = maxStamina;
    }
    public void ModifyStamina(float value)
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
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            staminaBar.SetValue(currentStamina);
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
    }


}
