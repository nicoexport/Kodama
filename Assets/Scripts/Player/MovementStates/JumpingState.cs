using Architecture;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.MovementStates
{
    public class JumpingState : AirbourneState
    {
        private float keepJumpingTimer;
        private bool keepJumping;

        public JumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
        {

        }


        private void Jump(float jumpForce)
        {
            if (character == null) return;
            character.transform.Translate(Vector2.up * (character.groundCheckRadius + 0.1f));
            var force = new Vector2(0f, jumpForce);
            character.rb.AddForce(force, ForceMode2D.Impulse);
        }

        private void StopJumping(InputAction.CallbackContext context)
        {
            keepJumping = false;
        }


        public override void Enter()
        {
            base.Enter();
            character.wantjump = false;
            InputManager.playerInputActions.Player.Jump.canceled += StopJumping;
            keepJumpingTimer = character.MovementValues.longJumpTimer;
            if (InputManager.playerInputActions.Player.Jump.ReadValue<float>() > 0f) keepJumping = true;
            Jump(character.MovementValues.jumpForce);
        }

        public override void Exit()
        {
            base.Exit();
            InputManager.playerInputActions.Player.Jump.canceled -= StopJumping;
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (character.rb.velocity.y < -0.1f) stateMachine.ChangeState(character.falling);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // makes the character keep jumping if the jump button is held
            if (keepJumping && keepJumpingTimer > 0f)
            {

                var force = new Vector2(0f, character.MovementValues.jumpForce * character.MovementValues.longJumpMultiplier);
                character.rb.AddForce(force, ForceMode2D.Force);

                keepJumpingTimer -= Time.deltaTime;
            }
        }
    }
}
