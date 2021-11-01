using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : State
{
    private float horizontalInput;
    private bool grounded;

    protected float speed;

    public GroundedState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        grounded = true;
        horizontalInput = 0.0f;
        character.ResetMoveParams();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        horizontalInput = Input.GetAxis("Horizontal");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!grounded) stateMachine.ChangeState(character.falling);
        if (horizontalInput > 0 && !character.facingRight) character.Flip();
        if (horizontalInput < 0 && character.facingRight) character.Flip();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontalInput, speed);
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        if (Mathf.Abs(character.rb.velocity.x) <= 1f && horizontalInput == 0f) character.rb.velocity = Vector2.zero;
    }

}
