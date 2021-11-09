using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandingState : GroundedState
{
    private bool jump;

    public StandingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        speed = character.movementSpeed;
        jump = false;
        character.playerInputActions.Player.Jump.performed += ChangeToJump;
    }

    public override void Exit()
    {
        base.Exit();
        character.playerInputActions.Player.Jump.performed -= ChangeToJump;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        // jump = Input.GetButtonDown("Jump");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (jump) stateMachine.ChangeState(character.jumping);
    }

    private void ChangeToJump(InputAction.CallbackContext context)
    {
        if (context.performed) stateMachine.ChangeState(character.jumping);
    }
}

