using UnityEngine;
using UnityEngine.Windows;

// Roll State
public class PlayerRollState : IPlayerState
{
    private PlayerController player;
    private float rollTimer;
    private Vector2 rollDirection;

    public PlayerRollState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //Debug.Log("Enter Roll State");

        rollTimer = player.GetRollDuration();

        float inputX = player.GetAnimator().GetFloat("InputX");
        float inputY = player.GetAnimator().GetFloat("InputY");
        rollDirection = new Vector2(inputX,inputY);

        
        player.PlayAnimation("Roll");
    }

    public void Update()
    {
        if (rollTimer > 0)
        {
            player.MovePlayer(rollDirection * player.GetRollSpeed());
            rollTimer -= Time.deltaTime;
        }
        else
        {
            player.ChangeState(new PlayerIdleState(player));
        }
    }

    public void Exit() 
    {
        
    }
}
