using UnityEngine;

public class SlimeAttackState : EnemyState
{
    private readonly EnemySlime enemy;

    public SlimeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (TriggerCalled)
            StateMachine.ChangeState(enemy.battleState);
    }
}