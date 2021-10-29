using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : State
{
    private bool grounded;
    private bool doubleJump;
    private bool fastFall;
    private float speed;
    private float horizontalInput;

    public FallingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        grounded = false;
        speed = character.airMovementSpeed;
        Debug.Log("Entered FALLING state");
    }

    public override void Exit()
    {
        base.Exit();
        character.rb.gravityScale = character.normalGravity;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        doubleJump = Input.GetButtonDown("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        fastFall = Input.GetKey("s");

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (grounded) stateMachine.ChangeState(character.standing);
        // if (!character.hasDoubleJumped && doubleJump) stateMachine.ChangeState(character.doubleJumping);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontalInput, speed);
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        // TO DO: mayby holding down will make you fall faster
        if (fastFall) character.rb.gravityScale = character.fastFallGravity;
        else
        {
            character.rb.gravityScale = character.normalGravity;
        }
    }
}

