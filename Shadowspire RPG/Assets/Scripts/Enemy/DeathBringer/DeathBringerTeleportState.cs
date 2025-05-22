public class DeathBringerTeleportState : EnemyState
{
    private readonly EnemyDeathBringer enemy;

    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemyDeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.MakeInvincible(true);
    }

    public override void Update()
    {
        base.Update();

        if (TriggerCalled)
        {
            if (enemy.CanDoSpellCast())
                StateMachine.ChangeState(enemy.spellCastState);
            else
                StateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stats.MakeInvincible(false);
    }
}