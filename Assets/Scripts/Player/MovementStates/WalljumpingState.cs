using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        character.transform.Translate(Vector2.left * (character.groundCheckRadius + 0.1f));
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

    public override void Enter()
    {
        base.Enter();
        Debug.Log("entered WALLJUMPING state");
        horizontalForce = character.horizontalWallJumpForce;
        verticalForce = character.verticalWallJumpForce;
        keepJumpingTimer = character.wallJumpTimer;
        WallJump(horizontalForce, verticalForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        keepJumping = Input.GetButton("Jump");
        if (Input.GetButtonUp("Jump")) keepJumpingTimer = 0f;
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
            var force = new Vector2(0f, verticalForce * 1.5f);
            character.rb.AddForce(force, ForceMode2D.Force);
            keepJumpingTimer -= Time.deltaTime;
        }
    }
}
