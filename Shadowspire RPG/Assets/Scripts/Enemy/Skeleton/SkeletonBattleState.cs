using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private static readonly int XVelocity = Animator.StringToHash("xVelocity");

    private Transform player;
    private readonly EnemySkeleton enemy;
    private int moveDir;

    private bool flippedOnce;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,
        EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            StateMachine.ChangeState(enemy.moveState);

        StateTimer = enemy.battleTime;
        flippedOnce = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.anim.SetFloat(XVelocity, enemy.rb.velocity.x);

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
            if (flippedOnce == false)
            {
                flippedOnce = true;
                enemy.Flip();
            }

            if (StateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                StateMachine.ChangeState(enemy.idleState);
        }

        float distanceToPlayerX = Mathf.Abs(player.position.x - enemy.transform.position.x);

        if (distanceToPlayerX < 1.5f)
        {
            return;
        }

        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

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