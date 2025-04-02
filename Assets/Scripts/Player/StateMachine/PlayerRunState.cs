using UnityEngine;

// Run State
public class PlayerRunState : IPlayerState
{
    private PlayerController player;

    public PlayerRunState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        if(player.GetMovement() == Vector2.zero)
        {
            return;
        }
        //Debug.Log("Enter Run State");
        player.PlayAnimation("Run");
    }

    public void Update()
    {
        Vector2 moveInput = player.GetMovement();
        if (moveInput != Vector2.zero)
        {
            player.PlayAnimation("Run");
            player.MovePlayer(moveInput * player.GetRunSpeed());
            player.SetAnimatorParameters(moveInput.x, moveInput.y);
        }
        else
        {
            player.ChangeState(new PlayerIdleState(player));
        }
    }

    public void Exit() { }
}