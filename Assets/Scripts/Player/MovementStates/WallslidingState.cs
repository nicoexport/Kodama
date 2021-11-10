using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        character.playerInputActions.Player.Jump.performed += ChangeToWallJumping;
        touchingWall = true;
        speed = character.movementSpeed;
        //For now with Gravity scale maybe use custom friction to slow down upwards momentum too
        character.rb.gravityScale = character.wallslidingGravity;
    }

    public override void Exit()
    {
        base.Exit();
        character.rb.gravityScale = character.normalGravity;
        character.playerInputActions.Player.Jump.performed -= ChangeToWallJumping;

    }

    public override void HandleInput()
    {
        base.HandleInput();
        horizontalInput = character.playerInputActions.Player.Movement.ReadValue<Vector2>().x;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!touchingWall) stateMachine.ChangeState(character.falling);
        if (grounded) stateMachine.ChangeState(character.standing);
        if (character.wantjump && touchingWall) stateMachine.ChangeState(character.wallJumping);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        touchingWall = character.CheckCollisionOverlap(character.frontCheck.position, character.frontCheckRadius);
        character.Move(horizontalInput, speed);
    }

    private void ChangeToWallJumping(InputAction.CallbackContext context)
    {
        if (touchingWall) stateMachine.ChangeState(character.wallJumping);
    }
}
