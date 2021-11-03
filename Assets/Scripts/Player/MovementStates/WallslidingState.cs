using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallslidingState : State
{
    private bool grounded;
    private bool touchingWall;

    private float horizontalInput;
    private float speed;

    private bool wallJump;

    public WallslidingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        wallJump = false;
        speed = character.movementSpeed;
        //For now with Gravity scale maybe use custom friction to slow down upwards momentum too
        character.rb.gravityScale = character.wallslidingGravity;
    }

    public override void Exit()
    {
        base.Exit();
        character.rb.gravityScale = character.normalGravity;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        horizontalInput = Input.GetAxis("Horizontal");
        wallJump = Input.GetButtonDown("Jump");
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        touchingWall = character.CheckCollisionOverlap(character.frontCheck.position, character.groundCheckRadius);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!touchingWall) stateMachine.ChangeState(character.falling);
        if (grounded) stateMachine.ChangeState(character.standing);
        if (wallJump) stateMachine.ChangeState(character.wallJumping);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontalInput, speed);
    }
}
