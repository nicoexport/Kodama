using Architecture;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.MovementStates
{
    public class WallslidingState : State
    {
        private bool grounded;
        private bool touchingWall;
        private float horizontalInput;
        private float speed;


        public WallslidingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
        {

        }

        public override void Enter()
        {
            base.Enter();
            InputManager.playerInputActions.Player.Jump.performed += ChangeToWallJumping;
            touchingWall = true;
            grounded = false;
            speed = character.movementSpeed;
            //For now with Gravity scale maybe use custom friction to slow down upwards momentum too
            character.rb.gravityScale = character.wallslidingGravity;
        }

        public override void Exit()
        {
            base.Exit();
            character.rb.gravityScale = character.normalGravity;
            InputManager.playerInputActions.Player.Jump.performed -= ChangeToWallJumping;

        }

        public override void HandleInput()
        {
            base.HandleInput();
            var hInput = InputManager.playerInputActions.Player.Movement.ReadValue<Vector2>().x;
            if (Mathf.Abs(hInput) >= 0.7f)
                horizontalInput = hInput;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (grounded) stateMachine.ChangeState(character.standing);
            else if (!touchingWall && !character.wantjump) stateMachine.ChangeState(character.falling);
            else if (character.wantjump && touchingWall) stateMachine.ChangeState(character.wallJumping);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            touchingWall = character.CheckCollisionOverlap(character.frontCheck.position, character.frontCheckRadius);
            grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
            character.Move(horizontalInput, speed);
        }

        private void ChangeToWallJumping(InputAction.CallbackContext context)
        {
            if (touchingWall) stateMachine.ChangeState(character.wallJumping);
        }
    }
}
