using UnityEngine;

public class ArcherDeadState : EnemyState
{
    private readonly EnemyArcher enemy;

    public ArcherDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        StateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer > 0)
            Rb.velocity = new Vector2(0, 10);
    }
}