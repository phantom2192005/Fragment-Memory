﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float runSpeed = 6.0f;
    [SerializeField] private float rollSpeed = 10.0f;
    [SerializeField] private float rollDuration = 0.5f; // Thời gian lăn

    // Public getter methods
    public float GetBaseSpeed() => baseSpeed;
    public float GetRunSpeed() => runSpeed;
    public float GetRollSpeed() => rollSpeed;
    public float GetRollDuration() => rollDuration;


    private Animator animator;

    public Vector2 moveInput;
    public IPlayerState currentState;

    public CoreCombat coreCombat;

    private InputAction walkAction;
    private InputAction runAction;
    private InputAction attackAction;

    private bool isRunning;
    public bool isAttacking;

    private Dictionary<Vector2Int, string> directionMapping = new Dictionary<Vector2Int, string>
{
    { new Vector2Int(1, 0), "Right" },
    { new Vector2Int(1, 1), "Right" },
    { new Vector2Int(1, -1), "Right" },

    { new Vector2Int(-1, 0), "Left" },
    { new Vector2Int(-1, 1), "Left" },
    { new Vector2Int(-1, -1), "Left" },

    { new Vector2Int(0, 1), "Up" },
    { new Vector2Int(0, -1), "Down" }
};

    private string lastAttackDirection = "Down"; // Khởi tạo hướng tấn công cuối cùng

    public string GetAttackDirection()
    {
        float inputX = animator.GetFloat("InputX");
        float inputY = animator.GetFloat("InputY");

        Vector2Int inputDir = new Vector2Int(Mathf.RoundToInt(inputX), Mathf.RoundToInt(inputY));

        if (inputDir != Vector2Int.zero && directionMapping.TryGetValue(inputDir, out string direction))
        {
            lastAttackDirection = direction;
        }

        return lastAttackDirection;
    }



    public IPlayerState GetCurrentState()
    {
        return currentState;
    }

    void Start()
    {
        InitializeComponents();
        InitializeInputActions();
        RegisterInputActions();

        // Set initial state
        ChangeState(new PlayerIdleState(this));
    }

    void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        coreCombat = GetComponentInChildren<CoreCombat>();

    }

    void InitializeInputActions()
    {
        walkAction = InputSystem.actions.FindAction("Walk");
        runAction = InputSystem.actions.FindAction("Run");
        attackAction = InputSystem.actions.FindAction("Attack");

    }
    void RegisterInputActions()
    {
        runAction.performed += ctx => ToggleRunning(true);
        runAction.canceled += ctx => ToggleRunning(false);
        attackAction.performed += ctx => Attack();
    }

    void Attack()
    {
        if (coreCombat != null && coreCombat.isCooldown == false)
        {
            //Debug.Log("There is core combat");
            ChangeState(new PlayerAttackState(this));
            isAttacking = true;
        }
    }
    void Update()
    {
        if (currentState is PlayerDeathState || currentState is PlayerAttackState)
        {
            return;
        }
        moveInput = walkAction.ReadValue<Vector2>();

        if (isRunning && currentState is PlayerIdleState)
        {
            ChangeState(new PlayerRunState(this));
        }

        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void ToggleRunning(bool state)
    {
        isRunning = state;
        if (currentState is PlayerWalkState)
        {
            if (state)
            {
                ChangeState(new PlayerRunState(this));
            }
        }
        else if (currentState is PlayerRunState)
        {
            if (!state)
            {
                ChangeState(new PlayerWalkState(this));
            }
        }
    }

    public void Roll(InputAction.CallbackContext context)
    {
        if (context.performed && !(currentState is PlayerRollState) && isAttacking == false)
        {
            ChangeState(new PlayerRollState(this));
        }
    }

    public Vector2 GetMovement()
    {
        return moveInput;
    }

    public void MovePlayer(Vector2 movement)
    {
        transform.position += (Vector3)(movement * Time.deltaTime);
    }
    public Animator GetAnimator()
    {
        return animator;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return this.GetComponent<SpriteRenderer>();
    }
    public void SetAnimatorParameters(float x, float y)
    {
        animator.SetFloat("InputX", x);
        animator.SetFloat("InputY", y);
    }

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    private void IsDead()
    {
        ChangeState(new PlayerDeathState(this));
    }
}