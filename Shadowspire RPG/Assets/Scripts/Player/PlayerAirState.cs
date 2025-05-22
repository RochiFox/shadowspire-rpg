public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Player.IsWallDetected())
            StateMachine.ChangeState(Player.wallSlide);

        if (Player.IsGroundDetected())
            StateMachine.ChangeState(Player.idleState);

        if (XInput != 0)
            Player.SetVelocity(Player.moveSpeed * .8f * XInput, Rb.velocity.y);
    }
}