using UnityEngine;

public class ShadyGroundedState : EnemyState
{
    private Transform player;
    protected readonly EnemyShady Enemy;

    protected ShadyGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemyShady _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        Enemy = _enemy;
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