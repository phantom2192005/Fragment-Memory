using UnityEngine;

public class WeaponAnimationEventHandler : MonoBehaviour
{
    PlayerController player;
    SpriteRenderer baseSpriteRender;
    Animator animator;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        baseSpriteRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void EnableSpriteRender()
    {
        baseSpriteRender.enabled = true;
        player.GetSpriteRenderer().enabled = false;
    }

    void TriggerCoolDown()
    {
        //Debug.Log("TriggerCoolDown is call");
        player.coreCombat.TriggerCoolDown();
    }

    void OnAttackAnimationEnd()
    {
        player.isAttacking = false;
        baseSpriteRender.enabled = false;
        player.GetSpriteRenderer().enabled = true;
        player.ChangeState(new PlayerIdleState(player));
    }

}
