using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
        player.StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }

    public void Update() { }

    public void Exit() { }
}
