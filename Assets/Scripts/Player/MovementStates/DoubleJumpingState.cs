using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpingState : AirbourneState
{
    private float horizontalInput;
    private float doubleJumpForce;
    private float speed;


    public DoubleJumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    private void DoubleJump()
    {
        // character.transform.Translate(Vector2.up * (character.groundCheckRadius + 0.1f));
        character.rb.velocity = new Vector2(character.rb.velocity.x, doubleJumpForce);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("entered DOUBLEJUMPING state");
        character.hasDoubleJumped = true;
        doubleJumpForce = character.doubleJumpForce;
        speed = character.movementSpeed;
        DoubleJump();
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontalInput, speed);
    }
}
