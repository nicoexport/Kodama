using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    [SerializeField]
    private string currentAnimState;

    private Animator animator;

    const string idle = "IDLE";
    const string running = "RUNNING";
    const string falling = "FALLING";
    const string jumping = "JUMPING";
    const string doubleJumping = "DOUBLEJUMPING";
    const string wallSliding = "WALLSLIDING";

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimationeState(State state, float horizontalVelocity)
    {
        string newAnimState = idle;

        switch (state.ToString())
        {
            case "StandingState":
                if (Mathf.Abs(horizontalVelocity) >= 0.1f) newAnimState = running;
                else newAnimState = idle;
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

            default:
                newAnimState = idle;
                break;
        }

        if (newAnimState == currentAnimState) return;
        animator.Play(newAnimState, 0);
        currentAnimState = newAnimState;
    }

}
