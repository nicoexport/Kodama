using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbourneState : State
{
    private float horizontalInput;
    private float airStrafeSpeed;
    private bool fastFall;
    private bool grounded;
    private bool touchingWall;

    public AirbourneState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        grounded = false;
        airStrafeSpeed = character.airMovementSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        horizontalInput = Input.GetAxis("Horizontal");
        fastFall = Input.GetKey("s");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (grounded) stateMachine.ChangeState(character.standing);
        if (character.rb.velocity.y < -0.1f && stateMachine.CurrentState != character.falling) stateMachine.ChangeState(character.falling);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // ground checking
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        // TO DO: Wallchecking
        // touchingWall = character.CheckCollisionOverlap(character.)
        // Air strafing
        character.Move(horizontalInput, airStrafeSpeed);
        // fast falling
        if (fastFall) character.rb.gravityScale = character.fastFallGravity;
        else
        {
            character.rb.gravityScale = character.normalGravity;
        }
    }
}
