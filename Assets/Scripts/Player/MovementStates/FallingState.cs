namespace Kodama.Player.MovementStates {
    public class FallingState : AirborneState {
        public FallingState(StateMachine stateMachine, Character character) : base(stateMachine, character) {
        }

        public override void Enter() => base.Enter();

        public override void Exit() => base.Exit();

        public override void HandleInput() => base.HandleInput();

        public override void LogicUpdate() => base.LogicUpdate();

        public override void PhysicsUpdate() => base.PhysicsUpdate();
    }
}