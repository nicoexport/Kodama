using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpingState : State
{
    private float horizontalInput;
    private float doubleJumpForce;

    public DoubleJumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    private void DoubleJump()
    {
        character.transform.Translate(Vector2.up * (character.groundCheckRadius + 0.1f));
        character.rb.velocity = new Vector2(horizontalInput * Time.fixedDeltaTime * character.movementSpeed * 10f, doubleJumpForce);
        // TO DO: Trigger the state change after a certain delay and put into LogicUpdate()
        stateMachine.ChangeState(character.falling);
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("entered DOUBLEJUMPING state");
        character.hasDoubleJumped = true;
        doubleJumpForce = character.doubleJumpForce;
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
}
