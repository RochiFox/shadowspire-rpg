using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    private readonly EnemyDeathBringer enemy;

    public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.chanceToTeleport += 5;
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
        {
            if (enemy.CanTeleport())
                StateMachine.ChangeState(enemy.teleportState);
            else
                StateMachine.ChangeState(enemy.battleState);
        }
    }
}