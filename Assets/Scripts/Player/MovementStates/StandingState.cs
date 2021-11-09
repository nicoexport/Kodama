using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandingState : GroundedState
{
    public StandingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Enter()
    {
        base.Enter();
        speed = character.movementSpeed;
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
    }

    private void ChangeToJump(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(character.jumping);
    }
}

