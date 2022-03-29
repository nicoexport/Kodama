using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using System;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    private string currentAnimState;
    public Animator animator { get; private set; }

    public event Action<string, string> OnAnimationStateChange;

    public const string idle = "IDLE";
    public const string running = "RUNNING";
    public const string falling = "FALLING";
    public const string jumping = "JUMPING";
    public const string doubleJumping = "DOUBLEJUMPING";
    public const string wallSliding = "WALLSLIDING";
    public const string wallJumping = "WALLJUMPING";
    public const string walkingAgainstWall = "WALKING_AGAINST_WALL";
    public const string spawning = "SPAWNING";
    public const string winning = "WINNING";
    public const string dying = "DYING";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationState(State state, float horizontalInput, float xVelocity, float maxVelocityX, bool touchingWall)
    {
        string newAnimState = idle;
        switch (state.ToString())
        {
            case "StandingState":
                newAnimState = idle;
                break;

            case "RunningState":
                if (touchingWall) newAnimState = walkingAgainstWall;
                else
                {
                    newAnimState = running;
                    animator.SetFloat("runSpeed", Mathf.Abs(xVelocity) / maxVelocityX);
                }
                break;

            case "FallingState":
                newAnimState = falling;
                break;

            case "JumpingState":
                newAnimState = jumping;
                break;

            case "DoubleJumpingState":
                newAnimState = doubleJumping;
                break;

            case "WallslidingState":
                newAnimState = wallSliding;
                break;

            case "WalljumpingState":
                newAnimState = wallJumping;
                break;

            case "SpawningState":
                newAnimState = spawning;
                break;
            case "WinningState":
                newAnimState = winning;
                break;
            case "DyingState":
                newAnimState = dying;
                break;
            default:
                newAnimState = idle;
                break;
        }

        if (newAnimState == currentAnimState) return;
        animator.Play(newAnimState, 0);
        OnAnimationStateChange?.Invoke(currentAnimState, newAnimState);
        currentAnimState = newAnimState;
    }
}
