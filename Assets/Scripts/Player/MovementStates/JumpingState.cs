using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : State
{
    private float speed;
    private float horizontalInput;
    private float keepJumpingTimer;
    private bool grounded;
    private bool keepJumping;
    private bool doubleJump;

    private float fallingTimer;
    private bool countdownFalling;

    public JumpingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }


    private void Jump(bool canLongJump, float jumpForce)
    {
        character.transform.Translate(Vector2.up * (character.groundCheckRadius + 0.1f));
        character.rb.velocity = new Vector2(character.rb.velocity.x, jumpForce);
        if (canLongJump)
        {
            keepJumping = Input.GetButton("Jump");
        }
    }
    private void StartFallingTimer()
    {
        countdownFalling = true;
    }

    public override void Enter()
    {
        base.Enter();
        keepJumpingTimer = character.longJumpTimer;
        speed = character.airMovementSpeed;
        Jump(true, character.jumpForce);
        grounded = false;
        countdownFalling = false;
        fallingTimer = 0.4f;
        Debug.Log("Entered jumping state");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        horizontalInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonUp("Jump")) keepJumping = false;
        doubleJump = Input.GetButtonDown("Jump");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // delay for entering the falling state when the jump button is not held
        if (!keepJumping || keepJumpingTimer <= 0) StartFallingTimer();
        if (grounded) stateMachine.ChangeState(character.standing);
        if (fallingTimer <= 0 && countdownFalling) stateMachine.ChangeState(character.falling);
        // if (!character.hasDoubleJumped && doubleJump) stateMachine.ChangeState(character.doubleJumping);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontalInput, speed);
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        // makes the character keep jumping if the jump button is held
        if (keepJumping && keepJumpingTimer > 0f)
        {
            character.rb.velocity = new Vector2(character.rb.velocity.x, character.jumpForce);
            keepJumpingTimer -= Time.deltaTime;
        }

        if (countdownFalling) fallingTimer -= Time.deltaTime;
    }
}
