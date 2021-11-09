using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbourneState : State
{
    private float horizontalInput;
    private float verticalInput;
    private float airStrafeSpeed;
    public bool grounded;
    public bool touchingWall;
    public bool touchingCeiling;

    public AirbourneState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        grounded = false;
        touchingWall = false;
        touchingCeiling = false;
        airStrafeSpeed = character.airMovementSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        horizontalInput = character.playerInputActions.Player.Movement.ReadValue<Vector2>().x;
        verticalInput = character.playerInputActions.Player.Movement.ReadValue<Vector2>().y;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (grounded) stateMachine.ChangeState(character.standing);
        if (touchingCeiling) stateMachine.ChangeState(character.falling);
        if (touchingWall)
        {
            if ((character.facingRight && horizontalInput > character.wallSlideInputThresh) || (!character.facingRight && horizontalInput < -character.wallSlideInputThresh)) stateMachine.ChangeState(character.wallsliding);
        }

        if (horizontalInput > 0 && !character.facingRight) character.Flip();
        if (horizontalInput < 0 && character.facingRight) character.Flip();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // ground checking
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        //wallchecking
        touchingWall = character.CheckCollisionOverlap(character.frontCheck.position, character.groundCheckRadius);
        // ceiling checking
        touchingCeiling = (character.CheckCollisionOverlap(character.ceilingCheck.position, character.ceilingCheckRadius)) || (character.CheckCollisionOverlap(character.ceilingCheck1.position, character.ceilingCheckRadius));
        // Air strafing
        character.Move(horizontalInput, airStrafeSpeed);
        // fast falling
        if (verticalInput <= -0.75f) character.rb.gravityScale = character.fastFallGravity;
        else if (stateMachine.CurrentState != character.wallsliding)
        {
            character.rb.gravityScale = character.normalGravity;
        }
    }
}
