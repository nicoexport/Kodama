using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    private string currentAnimState;
    private Animator animator;

    const string idle = "IDLE";
    const string running = "RUNNING";
    const string falling = "FALLING";
    const string jumping = "JUMPING";
    const string doubleJumping = "DOUBLEJUMPING";
    const string wallSliding = "WALLSLIDING";
    const string wallJumping = "WALLJUMPING";
    const string walkingAgainstWall = "WALKING_AGAINST_WALL";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationeState(State state, float horizontalInput, float xVelocity, float maxVelocityX, bool touchingWall)
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

            default:
                newAnimState = idle;
                break;
        }

        if (newAnimState == currentAnimState) return;
        animator.Play(newAnimState, 0);
        currentAnimState = newAnimState;
    }

}
