using UnityEngine;

public class ArcherJumpState : EnemyState
{
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    private readonly EnemyArcher enemy;

    public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyArcher _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        Rb.velocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat(YVelocity, Rb.velocity.y);

        if (Rb.velocity.y < 0 && enemy.IsGroundDetected())
            StateMachine.ChangeState(enemy.battleState);
    }
}