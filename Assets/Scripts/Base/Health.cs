using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth = 50;
    [SerializeField] public int currentHealth = 0;
    Animator animator;
    public HealthBar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        if(healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    private void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        
        currentHealth -= amount;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            animator.Play("Death");
        }
    }
}
