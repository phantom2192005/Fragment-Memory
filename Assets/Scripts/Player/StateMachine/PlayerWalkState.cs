using UnityEngine;

public class PlayerWalkState : IPlayerState
{
    private PlayerController player;
    private int currentWalkSoundIndex = 0;
    private float soundCooldownTimer = 0f;
    private const float WALK_SOUND_COOLDOWN = 0.3f; // Cooldown mặc định 0.2s

    public PlayerWalkState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.PlayAnimation("Walk");
        currentWalkSoundIndex = 0;
        soundCooldownTimer = 0f;
    }

    public void Update()
    {
        Vector2 moveInput = player.GetMovement();

        // Xử lý cooldown
        if (soundCooldownTimer > 0)
        {
            soundCooldownTimer -= Time.deltaTime;
        }

        if (moveInput != Vector2.zero)
        {
            player.PlayAnimation("Walk");
            player.MovePlayer(moveInput * player.GetBaseSpeed());
            player.SetAnimatorParameters(moveInput.x, moveInput.y);

            // Chỉ phát âm thanh khi hết cooldown
            if (soundCooldownTimer <= 0)
            {
                PlayNextWalkSound();
                soundCooldownTimer = WALK_SOUND_COOLDOWN; // Reset cooldown
            }
        }
        else
        {
            player.ChangeState(new PlayerIdleState(player));
        }
    }

    private void PlayNextWalkSound()
    {
        string soundKey = $"Walk_{currentWalkSoundIndex + 1}";

        if (SFXManager.Instance.SFX.TryGetValue(soundKey, out AudioClip clip))
        {
            // Sử dụng AudioSource của player thay vì PlayClipAtPoint để kiểm soát tốt hơn
              AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position,0.2f);

            // Tăng index và loop lại
            currentWalkSoundIndex = (currentWalkSoundIndex + 1) % GetWalkSoundCount();
        }
        else
        {
            currentWalkSoundIndex = 0; // Reset nếu không tìm thấy sound
        }
    }

    private int GetWalkSoundCount()
    {
        int count = 0;
        while (SFXManager.Instance.SFX.ContainsKey($"Walk_{count + 1}"))
        {
            count++;
        }
        return Mathf.Max(1, count);
    }

    public void Exit()
    {
    }
}