using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = Player.sword.transform;

        Player.fx.PlayDustFX();
        Player.fx.ScreenShake(Player.fx.shakeSwordImpact);

        if (Player.transform.position.x > sword.position.x && Player.facingDir == 1 ||
            Player.transform.position.x < sword.position.x && Player.facingDir == -1)
            Player.Flip();

        Rb.velocity = new Vector2(Player.swordReturnImpact * -Player.facingDir, Rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        Player.StartCoroutine(nameof(global::Player.BusyFor), .1f);
    }

    public override void Update()
    {
        base.Update();

        if (TriggerCalled)
            StateMachine.ChangeState(Player.idleState);
    }
}