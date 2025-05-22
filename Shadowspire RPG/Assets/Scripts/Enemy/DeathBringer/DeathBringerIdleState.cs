using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    private readonly EnemyDeathBringer enemy;
    private Transform player;

    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        StateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 7)
            enemy.bossFightBegun = true;

        if (Input.GetKeyDown(KeyCode.V))
            StateMachine.ChangeState(enemy.teleportState);

        if (StateTimer < 0 && enemy.bossFightBegun)
            StateMachine.ChangeState(enemy.battleState);
    }
}