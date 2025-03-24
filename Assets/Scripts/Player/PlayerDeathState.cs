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
        //Debug.Log("Enter Death State");
        player.PlayAnimation("Death");
      
    }

    public void Update() 
    {

    }
    public void Exit()
    {
        
    }

}
