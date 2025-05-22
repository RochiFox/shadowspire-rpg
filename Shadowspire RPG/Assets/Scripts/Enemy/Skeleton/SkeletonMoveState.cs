public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Update()
    {
        base.Update();

        Enemy.SetVelocity(Enemy.moveSpeed * Enemy.facingDir, Rb.velocity.y);

        if (Enemy.IsWallDetected() || !Enemy.IsGroundDetected())
        {
            Enemy.Flip();
            StateMachine.ChangeState(Enemy.idleState);
        }
    }
}