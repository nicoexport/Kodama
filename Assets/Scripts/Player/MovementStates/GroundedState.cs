using Kodama.Architecture;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kodama.Player.MovementStates {
    public class GroundedState : State {
        private float additionalDrag;
        private bool grounded;
        private float horizontalInput;
        protected bool running;
        protected float speed;
        private bool sprinting;
        protected float sprintSpeed;

        public GroundedState(StateMachine stateMachine, Character character) : base(stateMachine, character) {
        }

        public override void Enter() {
            base.Enter();
            additionalDrag = character.MovementValues.groundDecelDrag;
            grounded = true;
            horizontalInput = 0.0f;
            InputManager.playerInputActions.Player.Jump.started += ChangeToJump;
            character.ResetMoveParams();
        }

        public override void Exit() {
            base.Exit();
            character.SetWasGroundedTrue();
            InputManager.playerInputActions.Player.Jump.started -= ChangeToJump;
        }

        public override void HandleInput() {
            base.HandleInput();
            horizontalInput = InputManager.GetHorizontalMovementValue();
            sprinting = InputManager.playerInputActions.Player.Sprinting.IsPressed();
        }

        public override void LogicUpdate() {
            base.LogicUpdate();
            if (!grounded) {
                stateMachine.ChangeState(character.falling);
            }

            if (horizontalInput > 0 && !character.facingRight) {
                character.Flip();
            }

            if (horizontalInput < 0 && character.facingRight) {
                character.Flip();
            }

            if (Mathf.Abs(horizontalInput) > 0f) {
                running = true;
            } else {
                running = false;
            }
        }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            character.Move(horizontalInput, sprinting ? sprintSpeed : speed);
            grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
            if (horizontalInput == 0) {
                Vector2 newVelocity;
                var velocity = character.rb.velocity;
                if (velocity.x < -0.5f) {
                    newVelocity = new Vector2(velocity.x + (Time.deltaTime * additionalDrag), velocity.y);
                } else if (velocity.x > 0.5f) {
                    newVelocity = new Vector2(velocity.x - (Time.deltaTime * additionalDrag), velocity.y);
                } else {
                    newVelocity = new Vector2(0f, velocity.y);
                }

                character.rb.velocity = newVelocity;
            }
        }

        private void ChangeToJump(InputAction.CallbackContext context) => stateMachine.ChangeState(character.jumping);
    }
}