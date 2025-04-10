using UnityEngine;

// Run State
public class PlayerRunState : IPlayerState
{
    private PlayerController player;

    private int currentRunSoundIndex = 0;
    private float soundCooldownTimer = 0f;
    private const float RUN_SOUND_COOLDOWN = 0.3f;

    public PlayerRunState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        if (player.GetMovement() == Vector2.zero)
        {
            return;
        }

        player.PlayAnimation("Run");
        currentRunSoundIndex = 0;
        soundCooldownTimer = 0f;
    }

    public void Update()
    {
        Vector2 moveInput = player.GetMovement();

        if (soundCooldownTimer > 0)
        {
            soundCooldownTimer -= Time.deltaTime;
        }

        if (moveInput != Vector2.zero)
        {
            player.PlayAnimation("Run");
            player.MovePlayer(moveInput * player.GetRunSpeed());
            player.SetAnimatorParameters(moveInput.x, moveInput.y);

            if (soundCooldownTimer <= 0)
            {
                PlayNextRunSound();
                soundCooldownTimer = RUN_SOUND_COOLDOWN;
            }
        }
        else
        {
            player.ChangeState(new PlayerIdleState(player));
        }
    }

    private void PlayNextRunSound()
    {
        string soundKey = $"Run_{currentRunSoundIndex + 1}";

        if (SFXManager.Instance.SFX.TryGetValue(soundKey, out AudioClip clip))
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.3f);
            currentRunSoundIndex = (currentRunSoundIndex + 1) % GetRunSoundCount();
        }
        else
        {
            currentRunSoundIndex = 0;
        }
    }

    private int GetRunSoundCount()
    {
        int count = 0;
        while (SFXManager.Instance.SFX.ContainsKey($"Run_{count + 1}"))
        {
            count++;
        }
        return Mathf.Max(1, count);
    }

    public void Exit() { }
}
