using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (Mathf.Approximately(XInput, Player.facingDir) && Player.IsWallDetected())
            return;

        if (XInput != 0 && !Player.isBusy)
            StateMachine.ChangeState(Player.moveState);
    }
}