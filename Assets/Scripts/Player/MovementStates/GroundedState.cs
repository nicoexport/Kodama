using Architecture;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.MovementStates
{
    public class GroundedState : State
    {
        private float horizontalInput;
        private float additionalDrag;

        protected bool grounded;
        protected float speed;
        protected bool running;

        public GroundedState(StateMachine stateMachine, Character character) : base(stateMachine, character)
        {

        }

        public override void Enter()
        {
            base.Enter();
            additionalDrag = character.MovementValues.groundDecelDrag;
            grounded = true;
            horizontalInput = 0.0f;
            InputManager.playerInputActions.Player.Jump.started += ChangeToJump;
            character.ResetMoveParams();
        }

        public override void Exit()
        {
            base.Exit();
            InputManager.playerInputActions.Player.Jump.started -= ChangeToJump;
        }

        public override void HandleInput()
        {
            base.HandleInput();
            horizontalInput = InputManager.GetHorizontalMovementValue();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!grounded) stateMachine.ChangeState(character.falling);
            if (horizontalInput > 0 && !character.facingRight) character.Flip();
            if (horizontalInput < 0 && character.facingRight) character.Flip();
            if (Mathf.Abs(horizontalInput) > 0f) running = true;
            else running = false;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            character.Move(horizontalInput, speed);
            grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
            if (horizontalInput == 0)
            {
                Vector2 newVelocity;
                var velocity = character.rb.velocity;
                if (character.rb.velocity.x < 0)
                {
                    newVelocity = new Vector2(velocity.x + Time.deltaTime * additionalDrag, velocity.y);
                }
                else
                {
                    newVelocity = new Vector2(velocity.x - Time.deltaTime * additionalDrag, velocity.y);
                }
                character.rb.velocity = newVelocity;
            }
        }

        private void ChangeToJump(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(character.jumping);
        }

    }
}
