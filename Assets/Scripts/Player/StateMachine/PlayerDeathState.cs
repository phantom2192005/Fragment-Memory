using UnityEngine;

public class PlayerDeathState : IPlayerState
{
    private PlayerController player;

    public PlayerDeathState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.PlayAnimation("Death");
    }

    public void Update() 
    {

    }
    public void Exit()
    {
        
    }

}
