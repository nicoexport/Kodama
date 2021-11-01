using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallslidingState : AirbourneState
{
    public WallslidingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Wallsliding State");
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
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!touchingWall) stateMachine.ChangeState(character.falling);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
