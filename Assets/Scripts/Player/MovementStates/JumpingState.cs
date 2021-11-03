using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : AirbourneState
{
    private float keepJumpingTimer;
    private bool keepJumping;
    private bool doubleJump;

    public JumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }


    private void Jump(bool canLongJump, float jumpForce)
    {
        character.transform.Translate(Vector2.up * (character.groundCheckRadius + 0.1f));
        var force = new Vector2(0f, jumpForce);
        character.rb.AddForce(force, ForceMode2D.Impulse);
        if (canLongJump)
        {
            keepJumping = Input.GetButton("Jump");
        }
    }

    public override void Enter()
    {
        base.Enter();
        keepJumpingTimer = character.longJumpTimer;
        Jump(true, character.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (Input.GetButtonUp("Jump")) keepJumping = false;
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

            var force = new Vector2(0f, character.jumpForce * character.longJumpMultiplier);
            character.rb.AddForce(force, ForceMode2D.Force);

            keepJumpingTimer -= Time.deltaTime;
        }
    }
}
