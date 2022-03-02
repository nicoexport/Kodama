using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public StateMachine stateMachine;
    public Character character;

    // Constructor for the state taking in a stateMachine its assigned to and a character its manipulating
    public State(StateMachine stateMachine, Character character)
    {
        this.stateMachine = stateMachine;
        this.character = character;
    }

    // Method getting called by the SM when entering the state
    public virtual void Enter()
    {
        if (character.debugStates) Debug.Log(this.ToString());
    }

    // Method getting called by the SM when exiting the state
    public virtual void Exit()
    {

    }

    // Method used to handle input getting called in the Update() method of the character
    public virtual void HandleInput()
    {

    }

    // Method used for updating the logic of the SM getting called in the Update() method of the character (change states here)
    public virtual void LogicUpdate()
    {

    }

    // Method used to manipulate the physics of the character. getting called in the FixedUpdate() method in the character
    public virtual void PhysicsUpdate()
    {

    }
}
