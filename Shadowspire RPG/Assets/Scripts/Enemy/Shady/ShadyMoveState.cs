public class ShadyMoveState : ShadyGroundedState
{
    public ShadyMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyShady _enemy) :
        base(_enemyBase, _stateMachine, _animBoolName, _enemy)
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