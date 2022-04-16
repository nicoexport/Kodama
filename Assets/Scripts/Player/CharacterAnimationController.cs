using System;
using UnityEngine;

namespace Player
{
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
            //print("Trying to set animation state");
            string newAnimState = idle;
            
            switch (state.ToString())
            {
                case "Player.MovementStates.StandingState":
                    newAnimState = idle;
                    break;

                case "Player.MovementStates.RunningState":
                    if (touchingWall) newAnimState = walkingAgainstWall;
                    else
                    {
                        newAnimState = running;
                        animator.SetFloat("runSpeed", Mathf.Abs(xVelocity) / maxVelocityX);
                    }
                    break;

                case "Player.MovementStates.FallingState":
                    newAnimState = falling;
                    break;

                case "Player.MovementStates.JumpingState":
                    newAnimState = jumping;
                    break;

                case "Player.MovementStates.DoubleJumpingState":
                    newAnimState = doubleJumping;
                    break;

                case "Player.MovementStates.WallslidingState":
                    newAnimState = wallSliding;
                    break;

                case "Player.MovementStates.WalljumpingState":
                    newAnimState = wallJumping;
                    break;

                case "Player.MovementStates.SpawningState":
                    newAnimState = spawning;
                    break;
                case "Player.MovementStates.WinningState":
                    newAnimState = winning;
                    break;
                case "Player.MovementStates.DyingState":
                    newAnimState = dying;
                    break;
                default:
                    newAnimState = idle;
                    break;
            }

            if (newAnimState == currentAnimState) return;
            animator.Play(newAnimState, 0);
            print(newAnimState);
            OnAnimationStateChange?.Invoke(currentAnimState, newAnimState);
            currentAnimState = newAnimState;
        }
    }
}
