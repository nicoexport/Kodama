using System;

namespace Player {
    public class StateMachine {
        public State CurrentState { get; private set; }
        public State PreviousState { get; private set; }
        public event Action<State> OnStateChanged;

        // Method used tho initialize the SM with a startingState
        public void Initialize(State startingState) {
            CurrentState = startingState;
            startingState.Enter();
            OnStateChanged?.Invoke(CurrentState);
        }

        // Method used to change to a new state calling the Enter() and Exit() methods respectively
        public void ChangeState(State newState) {
            CurrentState.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            newState.Enter();
            OnStateChanged?.Invoke(CurrentState);
        }
    }
}