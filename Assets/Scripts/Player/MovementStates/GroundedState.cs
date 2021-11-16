using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : State
{
    private float horizontalInput;
    private bool grounded;
    private float additionalDrag;

    protected float speed;
    protected bool running;

    public GroundedState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        additionalDrag = character.groundDecelDrag;
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
        horizontalInput = character.playerInputActions.Player.Movement.ReadValue<Vector2>().x;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (!grounded) stateMachine.ChangeState(character.falling);
        if (horizontalInput > 0 && !character.facingRight) character.Flip();
        if (horizontalInput < 0 && character.facingRight) character.Flip();
        if (Mathf.Abs(horizontalInput) > 0f) running = true;
        else running = false;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        character.Move(horizontalInput, speed);
        grounded = character.CheckCollisionOverlap(character.groundCheck.position, character.groundCheckRadius);
        // if (Mathf.Abs(character.rb.velocity.x) <= stoppingThresh && horizontalInput == 0f) character.rb.velocity = Vector2.zero;
        if (horizontalInput == 0)
        {
            var newVelocity = new Vector2();
            if (character.rb.velocity.x < 0)
            {
                newVelocity = new Vector2(character.rb.velocity.x + Time.deltaTime * additionalDrag, character.rb.velocity.y);
            }
            else
            {
                newVelocity = new Vector2(character.rb.velocity.x - Time.deltaTime * additionalDrag, character.rb.velocity.y);
            }
            character.rb.velocity = newVelocity;
        }
    }

}
