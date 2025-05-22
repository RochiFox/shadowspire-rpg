using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected readonly EnemySkeleton Enemy;
    private Transform player;

    protected SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.Enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (Enemy.IsPlayerDetected() ||
            Vector2.Distance(Enemy.transform.position, player.transform.position) < Enemy.agroDistance)
        {
            StateMachine.ChangeState(Enemy.battleState);
        }
    }
}