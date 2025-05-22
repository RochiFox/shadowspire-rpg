using UnityEngine;

public class ArcherGroundedState : EnemyState
{
    private Transform player;
    protected readonly EnemyArcher Enemy;

    protected ArcherGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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