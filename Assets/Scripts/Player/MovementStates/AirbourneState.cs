using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbourneState : State
{
    private float horizontalInput;
    private float airStrafeSpeed;
    private bool fastFall;
    private bool grounded;

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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        character.Move(horizontalInput, airStrafeSpeed);
        if (fastFall) character.rb.gravityScale = character.fastFallGravity;
        else
        {
            character.rb.gravityScale = character.normalGravity;
        }
    }
}
