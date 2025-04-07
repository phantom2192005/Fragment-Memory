using UnityEngine;

// Walk State
public class PlayerWalkState : IPlayerState
{
    private PlayerController player;

    public PlayerWalkState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //Debug.Log("Enter Walk State");
        player.PlayAnimation("Walk");
    }

    public void Update()
    {
        Vector2 moveInput = player.GetMovement();
        if (moveInput != Vector2.zero)
        {
            player.PlayAnimation("Walk");
            player.MovePlayer(moveInput * player.GetBaseSpeed());
            player.SetAnimatorParameters(moveInput.x, moveInput.y);
        }
        else if(moveInput == Vector2.zero)
        {
            player.ChangeState(new PlayerIdleState(player));
        }
    }

    public void Exit() 
    {
        
    }
}