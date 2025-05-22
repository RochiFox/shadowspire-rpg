using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Player.IsWallDetected() == false)
            StateMachine.ChangeState(Player.airState);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.ChangeState(Player.wallJump);
            return;
        }

        if (XInput != 0 && !Mathf.Approximately(Player.facingDir, XInput))
            StateMachine.ChangeState(Player.idleState);

        Rb.velocity = YInput < 0 ? new Vector2(0, Rb.velocity.y) : new Vector2(0, Rb.velocity.y * .7f);

        if (Player.IsGroundDetected())
            StateMachine.ChangeState(Player.idleState);
    }
}