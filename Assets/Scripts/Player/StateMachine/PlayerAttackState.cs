using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerController player;
    public PlayerAttackState(PlayerController player)
    {
        this.player = player;
    }
    public void Enter()
    {
        //Debug.Log("Enter Attack State");
        player.coreCombat.Attack();
    }

    public void Update()
    {

    }

    public void Exit()
    {
        //Debug.Log("Exit Attack State");
    }
}
