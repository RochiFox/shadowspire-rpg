using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Rb.velocity = new Vector2(Rb.velocity.x, Player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (Rb.velocity.y < 0)
            StateMachine.ChangeState(Player.airState);
    }
}