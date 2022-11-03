using System;
using UnityEngine;

namespace Kodama.Player {
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour {
        public const string idle = "IDLE";
        public const string running = "RUNNING";
        public const string falling = "FALLING";
        public const string fallingTransition = "FALLINGTRANSITION";
        public const string jumping = "JUMPING";
        public const string doubleJumping = "DOUBLEJUMPING";
        public const string wallSliding = "WALLSLIDING";
        public const string wallJumping = "WALLJUMPING";
        public const string walkingAgainstWall = "WALKING_AGAINST_WALL";
        public const string spawning = "SPAWNING";
        public const string winning = "WINNING";
        public const string dying = "DYING";
        public const string landing = "LANDING";
        private static readonly int RunSpeed = Animator.StringToHash("runSpeed");
        private string _currentAnimState;
        public float Speed { get; private set; }
        public Animator animator { get; private set; }

        private void Awake() => animator = GetComponent<Animator>();

        public event Action<string, string, float> OnAnimationStateChange;

        public void SetAnimationState(State state, float horizontalInput, float xVelocity, float maxVelocityX,
            bool touchingWall) {
            string newAnimState = idle;
            var currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            Speed = Mathf.Abs(xVelocity / maxVelocityX);
            switch (state.ToString()) {
                case "Kodama.Player.MovementStates.StandingState":

                    if (currentAnimatorStateInfo.IsName(falling) || currentAnimatorStateInfo.IsName(landing)) {
                        newAnimState = landing;
                    } else {
                        newAnimState = idle;
                    }

                    break;

                case "Kodama.Player.MovementStates.RunningState":
                    if (currentAnimatorStateInfo.IsName(falling) || currentAnimatorStateInfo.IsName(landing)) {
                        newAnimState = landing;
                        break;
                    }

                    if (touchingWall) {
                        newAnimState = walkingAgainstWall;
                    } else {
                        newAnimState = running;
                        animator.SetFloat(RunSpeed, Speed);
                    }

                    break;

                case "Kodama.Player.MovementStates.FallingState":
                    newAnimState = fallingTransition;
                    break;

                case "Kodama.Player.MovementStates.JumpingState":
                    newAnimState = jumping;
                    break;

                case "Kodama.Player.MovementStates.DoubleJumpingState":
                    newAnimState = doubleJumping;
                    break;

                case "Kodama.Player.MovementStates.WallslidingState":
                    newAnimState = wallSliding;
                    break;

                case "Kodama.Player.MovementStates.WalljumpingState":
                    newAnimState = wallJumping;
                    break;

                case "Kodama.Player.MovementStates.SpawningState":
                    newAnimState = spawning;
                    break;
                case "Kodama.Player.MovementStates.WinningState":
                    newAnimState = winning;
                    break;
                case "Kodama.Player.MovementStates.DyingState":
                    newAnimState = dying;
                    break;
                default:
                    newAnimState = idle;
                    break;
            }

            if (newAnimState == _currentAnimState) {
                return;
            }

            animator.Play(newAnimState, 0);
            OnAnimationStateChange?.Invoke(_currentAnimState, newAnimState, Speed);
            _currentAnimState = newAnimState;
        }
    }
}