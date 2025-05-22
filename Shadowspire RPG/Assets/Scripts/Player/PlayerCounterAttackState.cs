using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private static readonly int SuccessfulCounterAttack = Animator.StringToHash("SuccessfulCounterAttack");

    private bool canCreateClone;

    private readonly Collider2D[] arrowResult = new Collider2D[5];

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(
        _player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        StateTimer = Player.counterAttackDuration;
        Player.anim.SetBool(SuccessfulCounterAttack, false);
    }

    public override void Update()
    {
        base.Update();

        Player.SetZeroVelocity();

        int size = Physics2D.OverlapCircleNonAlloc(Player.attackCheck.position, Player.attackCheckRadius, arrowResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = arrowResult[i];

            if (hit.GetComponent<ArrowController>())
            {
                hit.GetComponent<ArrowController>().FlipArrow();
                CounterAttack();
            }

            if (hit.GetComponent<Enemy>())
            {
                Enemy enemy = hit.GetComponent<Enemy>();

                if (enemy.CanBeStunned())
                {
                    CounterAttack();

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        Player.skill.parry.MakeMirageOnParry(hit.transform);
                    }

                    Player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
                }
            }
        }

        if (StateTimer < 0 || TriggerCalled)
            StateMachine.ChangeState(Player.idleState);
    }

    private void CounterAttack()
    {
        StateTimer = 10; // any value bigger than 1
        Player.anim.SetBool(SuccessfulCounterAttack, true);
    }
}