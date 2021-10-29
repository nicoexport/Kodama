using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : AirbourneState
{
    private float speed;
    private float keepJumpingTimer;
    private bool keepJumping;
    private bool doubleJump;

    public JumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }


    private void Jump(bool canLongJump, float jumpForce)
    {
        character.transform.Translate(Vector2.up * (character.groundCheckRadius + 0.1f));
        character.rb.velocity = new Vector2(character.rb.velocity.x, jumpForce);
        if (canLongJump)
        {
            keepJumping = Input.GetButton("Jump");
        }
    }

    public override void Enter()
    {
        base.Enter();
        keepJumpingTimer = character.longJumpTimer;
        speed = character.airMovementSpeed;
        Jump(true, character.jumpForce);
        Debug.Log("Entered jumping state");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (Input.GetButtonUp("Jump")) keepJumping = false;
        doubleJump = Input.GetButtonDown("Jump") && character.canDoubleJump;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!character.hasDoubleJumped && doubleJump) stateMachine.ChangeState(character.doubleJumping);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // makes the character keep jumping if the jump button is held
        if (keepJumping && keepJumpingTimer > 0f)
        {
            character.rb.velocity = new Vector2(character.rb.velocity.x, character.jumpForce);
            keepJumpingTimer -= Time.deltaTime;
        }
    }
}
