using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : AirbourneState
{
    private bool doubleJump;
    private bool fastFall;

    public FallingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered FALLING state");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        doubleJump = Input.GetButtonDown("Jump") && character.canDoubleJump;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!character.hasDoubleJumped && doubleJump) stateMachine.ChangeState(character.doubleJumping);
        if (touchingWall) stateMachine.ChangeState(character.wallsliding);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        touchingWall = character.CheckCollisionOverlap(character.frontCheck.position, character.groundCheckRadius);
    }
}

