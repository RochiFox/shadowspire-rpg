public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StateTimer = Enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.PlaySfx(14, Enemy.transform);
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
            StateMachine.ChangeState(Enemy.moveState);
    }
}