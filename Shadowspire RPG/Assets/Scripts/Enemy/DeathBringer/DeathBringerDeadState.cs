using UnityEngine;

public class DeathBringerDeadState : EnemyState
{
    private readonly EnemyDeathBringer enemy;

    public DeathBringerDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        StateTimer = 0.15f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer > 0)
            Rb.velocity = new Vector2(0, 10);
    }
}