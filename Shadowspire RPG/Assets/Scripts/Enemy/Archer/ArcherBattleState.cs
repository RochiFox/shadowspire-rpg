using UnityEngine;

public class ArcherBattleState : EnemyState
{
    private Transform player;
    private readonly EnemyArcher enemy;
    private int moveDir;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemyArcher _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

            if (enemy.IsPlayerDetected().distance < enemy.safeDistance)
            {
                if (CanJump())
                    StateMachine.ChangeState(enemy.jumpState);
            }

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

        BattleStateFlipControl();
    }

    private void BattleStateFlipControl()
    {
        if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1 ||
            player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            enemy.Flip();
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

    private bool CanJump()
    {
        if (enemy.GroundBehind() == false || enemy.WallBehind())
            return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }
}