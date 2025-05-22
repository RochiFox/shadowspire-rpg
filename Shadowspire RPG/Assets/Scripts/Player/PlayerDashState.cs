public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.skill.dash.CloneOnDash();
        StateTimer = Player.dashDuration;

        Player.stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();

        Player.skill.dash.CloneOnArrival();
        Player.SetVelocity(0, Rb.velocity.y);

        Player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (!Player.IsGroundDetected() && Player.IsWallDetected())
            StateMachine.ChangeState(Player.wallSlide);

        Player.SetVelocity(Player.dashSpeed * Player.dashDir, 0);

        if (StateTimer < 0)
            StateMachine.ChangeState(Player.idleState);

        Player.fx.CreateAfterImage();
    }
}