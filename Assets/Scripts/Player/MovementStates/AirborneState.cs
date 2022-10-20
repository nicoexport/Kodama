using Architecture;
using UnityEngine.InputSystem;

namespace Player.MovementStates
{
    public class AirborneState : State
    {
        private float airStrafeSpeed;
        private bool grounded;
        private float horizontalInput;
        private bool sprinting;

        private float sprintSpeed;
        private bool touchingCeiling;
        protected bool touchingWall;
        private float verticalInput;

        public AirborneState(StateMachine stateMachine, Character character) : base(stateMachine, character)
        {
        }

        public override void Enter()
        {
            base.Enter();
            grounded = false;
            touchingWall = false;
            touchingCeiling = false;
            airStrafeSpeed = character.MovementValues.airMoveSpeed;
            sprintSpeed = character.MovementValues.sprintSpeed;
            InputManager.playerInputActions.Player.Jump.started += ChangeToJump;
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
            verticalInput = InputManager.GetVerticalMovementValue();
            sprinting = InputManager.playerInputActions.Player.Sprinting.IsPressed();
            // starting input timer for horizontal input
            if (horizontalInput > 0f)
            {
                character.hasPressedRight = true;
                character.hasPressedRightTimer = character.MovementValues.horizontalInputTimer;
            }

            if (horizontalInput < 0f)
            {
                character.hasPressedLeft = true;
                character.hasPressedLeftTimer = character.MovementValues.horizontalInputTimer;
            }
        } // ReSharper disable Unity.PerformanceAnalysis
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (grounded) stateMachine.ChangeState(character.standing);
            else if (touchingCeiling) stateMachine.ChangeState(character.falling);
            else if (touchingWall)
                if ((character.facingRight && character.hasPressedRight) ||
                    (!character.facingRight && character.hasPressedLeft))
                    stateMachine.ChangeState(character.wallsliding);

            if (horizontalInput > 0 && !character.facingRight) character.Flip();
            if (horizontalInput < 0 && character.facingRight) character.Flip();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // ground checking
            grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
            //wall checking
            touchingWall = character.CheckCollisionOverlap(character.frontCheck.position, character.frontCheckRadius);
            // ceiling checking
            touchingCeiling =
                character.CheckCollisionOverlap(character.ceilingCheck.position, character.ceilingCheckRadius) ||
                character.CheckCollisionOverlap(character.ceilingCheck1.position, character.ceilingCheckRadius);
            // Air strafing
            character.Move(horizontalInput, sprinting ? sprintSpeed : airStrafeSpeed);
            // fast falling
            if (verticalInput <= -0.75f) character.rb.gravityScale = character.MovementValues.fastFallGravity;
            else if (stateMachine.CurrentState != character.wallsliding)
                character.rb.gravityScale = character.MovementValues.normalGravity;
        }

        private void ChangeToJump(InputAction.CallbackContext context)
        {
            if (!character.wasGrounded || stateMachine.CurrentState == character.jumping)
                return;
            stateMachine.ChangeState(character.jumping);
        }
    }
}