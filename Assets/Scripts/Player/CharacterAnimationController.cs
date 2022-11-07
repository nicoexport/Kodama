using System;
using UnityEngine;

namespace Kodama.Player {
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour {
        public const string IDLE = "IDLE";
        public const string RUNNING = "RUNNING";
        public const string FALLING = "FALLING";
        public const string FALLING_TRANSITION = "FALLINGTRANSITION";
        public const string JUMPING = "JUMPING";
        public const string DOUBLE_JUMPING = "DOUBLEJUMPING";
        public const string WALL_SLIDING = "WALLSLIDING";
        public const string WALL_JUMPING = "WALLJUMPING";
        public const string WALKING_AGAINST_WALL = "WALKING_AGAINST_WALL";
        public const string SPAWNING = "SPAWNING";
        public const string WINNING = "WINNING";
        public const string DYING = "DYING";
        public const string LANDING = "LANDING";
        private static readonly int RunSpeed = Animator.StringToHash("runSpeed");
        [SerializeField] private float minimumAnimationSpeed = 0.1f;
        private string _currentAnimState;
        public float speed { get; private set; }
        private Animator Animator { get; set; }

        private void Awake() => Animator = GetComponent<Animator>();

        public event Action<string, string, float> onOnAnimationStateChange;

        public void SetAnimationState(State state, float horizontalInput, float xVelocity, float maxVelocityX,
            bool touchingWall) {
            string newAnimState = IDLE;
            var currentAnimatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            float intendedSpeed = Mathf.Abs(xVelocity / maxVelocityX);
            speed = Mathf.Clamp(intendedSpeed, minimumAnimationSpeed, Mathf.Infinity);
            switch (state.ToString()) {
                case "Kodama.Player.MovementStates.StandingState":

                    if (currentAnimatorStateInfo.IsName(FALLING) || currentAnimatorStateInfo.IsName(LANDING)) {
                        newAnimState = LANDING;
                    } else {
                        newAnimState = IDLE;
                    }

                    break;

                case "Kodama.Player.MovementStates.RunningState":
                    if (currentAnimatorStateInfo.IsName(FALLING) || currentAnimatorStateInfo.IsName(LANDING)) {
                        newAnimState = LANDING;
                        break;
                    }

                    if (touchingWall) {
                        newAnimState = WALKING_AGAINST_WALL;
                    } else {
                        newAnimState = RUNNING;
                        Animator.SetFloat(RunSpeed, speed);
                    }

                    break;

                case "Kodama.Player.MovementStates.FallingState":
                    newAnimState = FALLING_TRANSITION;
                    break;

                case "Kodama.Player.MovementStates.JumpingState":
                    newAnimState = JUMPING;
                    break;

                case "Kodama.Player.MovementStates.DoubleJumpingState":
                    newAnimState = DOUBLE_JUMPING;
                    break;

                case "Kodama.Player.MovementStates.WallslidingState":
                    newAnimState = WALL_SLIDING;
                    break;

                case "Kodama.Player.MovementStates.WalljumpingState":
                    newAnimState = WALL_JUMPING;
                    break;

                case "Kodama.Player.MovementStates.SpawningState":
                    newAnimState = SPAWNING;
                    break;
                case "Kodama.Player.MovementStates.WinningState":
                    newAnimState = WINNING;
                    break;
                case "Kodama.Player.MovementStates.DyingState":
                    newAnimState = DYING;
                    break;
                default:
                    newAnimState = IDLE;
                    break;
            }

            if (newAnimState == _currentAnimState) {
                return;
            }

            Animator.Play(newAnimState, 0);
            onOnAnimationStateChange?.Invoke(_currentAnimState, newAnimState, speed);
            _currentAnimState = newAnimState;
        }
    }
}