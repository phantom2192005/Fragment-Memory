using UnityEngine;

// Idle State
public class PlayerIdleState : IPlayerState
{
    private PlayerController player;

    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //Debug.Log("Enter Idle State");
        player.PlayAnimation("Idle");
    }

    public void Update()
    {
        Vector2 moveInput = player.GetMovement();
        if (moveInput != Vector2.zero)
        {
            player.ChangeState(new PlayerWalkState(player));
        }
    }

    public void Exit() 
    {

    }
}