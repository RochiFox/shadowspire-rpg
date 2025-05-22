using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private readonly EnemySlime enemy;
    private Transform player;
    private int moveDir;

    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySlime _enemy)
        : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            StateMachine.ChangeState(enemy.moveState);
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            StateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    StateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (StateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                StateMachine.ChangeState(enemy.idleState);
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if (enemy.IsPlayerDetected() && enemy.IsPlayerDetected().distance < enemy.attackDistance - .1f)
            return;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, Rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}