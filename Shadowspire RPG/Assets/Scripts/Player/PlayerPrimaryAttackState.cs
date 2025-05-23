using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private static readonly int ComboCounter = Animator.StringToHash("ComboCounter");

    public int comboCounter { get; private set; }

    private float lastTimeAttacked;
    private const float COMBO_WINDOW = 2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(
        _player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        XInput = 0;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + COMBO_WINDOW)
            comboCounter = 0;

        Player.anim.SetInteger(ComboCounter, comboCounter);

        float attackDir = Player.facingDir;

        if (XInput != 0)
            attackDir = XInput;

        Player.SetVelocity(Player.attackMovement[comboCounter].x * attackDir, Player.attackMovement[comboCounter].y);

        StateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        Player.StartCoroutine(nameof(global::Player.BusyFor), .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
            Player.SetZeroVelocity();

        if (TriggerCalled)
            StateMachine.ChangeState(Player.idleState);
    }
}