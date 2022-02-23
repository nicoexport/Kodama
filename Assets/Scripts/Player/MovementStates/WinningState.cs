using UnityEngine;
using System;

public class WinningState : State
{
    private bool grounded;

    public WinningState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        // stop all player Movement / set velocity to 0 
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        if (grounded) character.rb.velocity = Vector2.zero;
    }
}