using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private readonly EnemySkeleton enemy;

    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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