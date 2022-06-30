namespace Player.MovementStates
{
    public class StandingState : GroundedState
    {
        public StandingState(StateMachine stateMachine, Character character) : base(stateMachine, character)
        {
        }

        public override void Enter()
        {
            base.Enter();
            speed = 0f;
            running = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (character.wantjump) stateMachine.ChangeState(character.jumping);
            else if (running && stateMachine.CurrentState != character.running)
                stateMachine.ChangeState(character.running);
        }
    }
}