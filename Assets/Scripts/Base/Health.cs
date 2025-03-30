using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth = 50;
    [SerializeField] public int currentHealth = 0;
    Animator animator;
    public HealthBar healthBar;

    private DamageFlash damageFlash;
    private KnockBack KnockBack;
    public bool canKnockBack;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInParent<Animator>();
        damageFlash = GetComponent<DamageFlash>();
        KnockBack = GetComponent<KnockBack>();

    }

    private void Update()
    {

    }

    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        currentHealth -= amount;
        // call damage Flash
        if (damageFlash != null)
        {
            damageFlash.CallDamageFlash();
        }
        // call knock back
        if (healthBar != null)
        {
            healthBar.SetValue(currentHealth);

        }
        if (currentHealth <= 0)
        {
            animator.Play("Death");
        }
        if (canKnockBack)
        {
            KnockBack.CallKnockback(hitDirection, Vector2.zero, Vector2.zero);
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
