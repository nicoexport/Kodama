using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("Entered standing state");
    }

    public override void HandleInput()
    {
        base.HandleInput();
        jump = Input.GetButtonDown("Jump");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (jump) stateMachine.ChangeState(character.jumping);
    }




}
