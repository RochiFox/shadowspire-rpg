public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StateTimer = 1f;
        Player.SetVelocity(5 * -Player.facingDir, Player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
            StateMachine.ChangeState(Player.airState);

        if (Player.IsGroundDetected())
            StateMachine.ChangeState(Player.idleState);
    }
}