using UnityEngine;

public class SlimeStunnedState : EnemyState
{
    private static readonly int StunFold = Animator.StringToHash("StunFold");

    private readonly EnemySlime enemy;

    public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        StateTimer = enemy.stunDuration;

        Rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if (Rb.velocity.y < .1f && enemy.IsGroundDetected())
        {
            enemy.fx.Invoke("CancelColorChange", 0);
            enemy.anim.SetTrigger(StunFold);
            enemy.stats.MakeInvincible(true);
        }

        if (StateTimer < 0)
            StateMachine.ChangeState(enemy.idleState);
    }
}