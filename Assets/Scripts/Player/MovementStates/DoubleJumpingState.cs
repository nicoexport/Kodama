using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpingState : State
{
    private float horizontalInput;
    private float doubleJumpForce;

    private float fallingTimer;
    private bool countdownFalling;

    public DoubleJumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    private void DoubleJump()
    {
        character.transform.Translate(Vector2.up * (character.groundCheckRadius + 0.1f));
        character.rb.velocity = new Vector2(horizontalInput * Time.fixedDeltaTime * character.movementSpeed * 10f, doubleJumpForce);
        countdownFalling = true;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("entered DOUBLEJUMPING state");
        character.hasDoubleJumped = true;
        doubleJumpForce = character.doubleJumpForce;
        fallingTimer = 0.4f;
        countdownFalling = false;
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
        if (countdownFalling) fallingTimer -= Time.deltaTime;
        if (fallingTimer <= 0) stateMachine.ChangeState(character.falling);
    }
}
