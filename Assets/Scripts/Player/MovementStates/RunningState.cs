namespace Player.MovementStates
{
    public class RunningState : GroundedState
    {
        public RunningState(StateMachine stateMachine, Character character) : base(stateMachine, character)
        {

        }

        public override void Enter()
        {
            base.Enter();
            speed = character.MovementValues.moveSpeed;
            sprintSpeed = character.MovementValues.sprintSpeed;
            running = true;
        }

        public override void Exit()
        {
            base.Exit();
            running = false;
        }

        public override void HandleInput()
        {
            base.HandleInput();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (character.wantjump) stateMachine.ChangeState(character.jumping);
            else if (!running) stateMachine.ChangeState(character.standing);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

    }
}
