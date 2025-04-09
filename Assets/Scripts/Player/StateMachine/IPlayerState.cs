using UnityEngine;

// State Interface
public interface IPlayerState
{
    void Enter();
    void Update();
    void Exit();
}