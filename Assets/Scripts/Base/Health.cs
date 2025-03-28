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
    }

    private void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (healthBar != null)
        {
            healthBar.SetValue(currentHealth);
        }
        if (currentHealth <= 0)
        {
            animator.Play("Death");
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (healthBar != null)
        {
            healthBar.SetValue(currentHealth);
            animator.Play("Heal_VFX");
        }
    }
}
