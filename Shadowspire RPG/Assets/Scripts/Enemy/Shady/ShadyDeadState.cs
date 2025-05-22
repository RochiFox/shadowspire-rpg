public class ShadyDeadState : EnemyState
{
    private readonly EnemyShady enemy;

    public ShadyDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemyShady _enemy) :
        base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Update()
    {
        base.Update();

        if (TriggerCalled)
            enemy.SelfDestroy();
    }
}