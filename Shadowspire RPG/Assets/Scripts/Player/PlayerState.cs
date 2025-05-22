using UnityEngine;

public class PlayerState
{
    private static readonly int YVelocity = Animator.StringToHash("yVelocity");

    protected readonly PlayerStateMachine StateMachine;
    protected readonly Player Player;

    protected Rigidbody2D Rb;

    protected float XInput;
    protected float YInput;
    private readonly string animBoolName;

    protected float StateTimer;
    protected bool TriggerCalled;

    protected PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.Player = _player;
        this.StateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        Player.anim.SetBool(animBoolName, true);
        Rb = Player.rb;
        TriggerCalled = false;
    }

    public virtual void Update()
    {
        StateTimer -= Time.deltaTime;

        XInput = Input.GetAxisRaw("Horizontal");
        YInput = Input.GetAxisRaw("Vertical");
        Player.anim.SetFloat(YVelocity, Rb.velocity.y);
    }

    public virtual void Exit()
    {
        Player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        TriggerCalled = true;
    }
}