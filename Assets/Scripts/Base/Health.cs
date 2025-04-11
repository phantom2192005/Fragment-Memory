using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 50;
    [SerializeField] public float currentHealth = 0;
    Animator animator;
    public HealthBar healthBar;
    public DropItem dropItem;

    public AudioClip HitSFX;
    public AudioClip DeathSFX;

    private DamageFlash damageFlash;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        damageFlash = GetComponent<DamageFlash>();
        dropItem = GetComponent<DropItem>();
    }

    private void Update()
    {

    }

    public void ModifyHealth(float amount)
    {
        if (amount > 0 && animator != null)
        {
            animator.Play("Heal_VFX");
        }
        currentHealth += amount;
        if (HitSFX != null && amount <0 )
        {
            AudioSource.PlayClipAtPoint(HitSFX, Camera.main.transform.position, 0.3f);
        }
        // call damage Flash
        if (damageFlash != null && amount < 0)
        {
            damageFlash.CallDamageFlash();
        }
        // call knock back
        if (healthBar != null)
        {
            Debug.Log("SetHealthBar is call");
            healthBar.SetValue((int)currentHealth);

        }
        if (currentHealth <= 0) 
        {
            if (DeathSFX != null)
            {
                AudioSource.PlayClipAtPoint(DeathSFX, Camera.main.transform.position, 0.3f);
            }
        }
        if (dropItem != null && currentHealth <= 0)
        {
            dropItem.DropLoot();
        }
    }
}
