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
        character.playerInputActions.Player.Jump.started += ChangeToJump;
    }

    public override void Exit()
    {
        base.Exit();
        character.playerInputActions.Player.Jump.started -= ChangeToJump;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (character.wantjump) stateMachine.ChangeState(character.jumping);
    }

    private void ChangeToJump(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(character.jumping);
    }
}

