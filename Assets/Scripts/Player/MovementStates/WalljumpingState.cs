using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalljumpingState : AirbourneState
{
    private float horizontalForce;
    private float verticalForce;
    private bool keepJumping;
    private float keepJumpingTimer;

    public WalljumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    private void WallJump(float horizontalForce, float verticalForce)
    {
        character.transform.Translate(Vector2.left * (character.frontCheckRadius + 0.1f));
        if (character.facingRight)
        {
            // character.rb.velocity = new Vector2(-horizontalForce, verticalForce);
            var force = new Vector2(-horizontalForce, verticalForce);
            character.rb.AddForce(force, ForceMode2D.Impulse);
        }
        else
        {
            // character.rb.velocity = new Vector2(horizontalForce, verticalForce);
            var force = new Vector2(horizontalForce, verticalForce);
            character.rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void StopJumping(InputAction.CallbackContext context)
    {
        keepJumping = false;
    }

    public override void Enter()
    {
        base.Enter();
        character.playerInputActions.Player.Jump.canceled += StopJumping;
        character.wantjump = false;
        touchingWall = false;
        horizontalForce = character.horizontalWallJumpForce;
        verticalForce = character.verticalWallJumpForce;
        keepJumpingTimer = character.wallJumpTimer;
        if (character.playerInputActions.Player.Jump.ReadValue<float>() > 0f) keepJumping = true;
        WallJump(horizontalForce, verticalForce);
    }

    public override void Exit()
    {
        base.Exit();
        character.playerInputActions.Player.Jump.canceled -= StopJumping;
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

        if (keepJumping && keepJumpingTimer > 0f)
        {
            var force = new Vector2(0f, verticalForce);
            character.rb.AddForce(force, ForceMode2D.Force);
            keepJumpingTimer -= Time.deltaTime;
        }
    }
}
