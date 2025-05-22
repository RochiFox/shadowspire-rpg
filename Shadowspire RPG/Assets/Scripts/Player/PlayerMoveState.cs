public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySfx(8, null);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSfx(8);
    }

    public override void Update()
    {
        base.Update();

        Player.SetVelocity(XInput * Player.moveSpeed, Rb.velocity.y);

        if (XInput == 0 || Player.IsWallDetected())
            StateMachine.ChangeState(Player.idleState);
    }
}