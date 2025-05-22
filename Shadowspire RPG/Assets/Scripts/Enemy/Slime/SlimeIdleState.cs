public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy) :
        base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StateTimer = Enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (StateTimer < 0)
            StateMachine.ChangeState(Enemy.moveState);
    }
}