using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        Player.StartCoroutine(nameof(global::Player.BusyFor), .2f);
    }

    public override void Update()
    {
        base.Update();

        Player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            StateMachine.ChangeState(Player.idleState);

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Player.transform.position.x > mousePosition.x && Player.facingDir == 1 ||
            Player.transform.position.x < mousePosition.x && Player.facingDir == -1)
            Player.Flip();
    }
}