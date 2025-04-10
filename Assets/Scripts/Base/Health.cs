using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth = 50;
    [SerializeField] public int currentHealth = 0;
    Animator animator;
    public HealthBar healthBar;

    private DamageFlash damageFlash;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        damageFlash = GetComponent<DamageFlash>();
    }

    private void Update()
    {

    }

    public void ModifyHealth(int amount)
    {
        if (amount > 0 && animator != null) 
        {
            animator.Play("Heal_VFX");
        }
        currentHealth += amount;
        // call damage Flash
        if (damageFlash != null && amount < 0 )
        {
            damageFlash.CallDamageFlash();
        }
        // call knock back
        if (healthBar != null)
        {
            healthBar.SetValue(currentHealth);

        }
    }
}
