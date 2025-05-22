using UnityEngine;

public class EnemyState
{
    protected readonly EnemyStateMachine StateMachine;
    private readonly Enemy enemyBase;
    protected Rigidbody2D Rb;

    private readonly string animBoolName;

    protected float StateTimer;
    protected bool TriggerCalled;

    protected EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.StateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        StateTimer -= Time.deltaTime;
    }

    public virtual void Enter()
    {
        TriggerCalled = false;
        Rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        TriggerCalled = true;
    }
}