using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private readonly float flyTime = .25f;
    private bool skillUsed;
    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player,
        _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = Player.rb.gravityScale;

        skillUsed = false;
        StateTimer = flyTime;
        Rb.gravityScale = 0;
        Player.stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();

        Player.rb.gravityScale = defaultGravity;
        Player.fx.MakeTransparent(false);
        Player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        switch (StateTimer)
        {
            case > 0:
                Rb.velocity = new Vector2(0, 15);
                break;
            case < 0:
            {
                Rb.velocity = new Vector2(0, -.1f);

                if (!skillUsed)
                {
                    if (Player.skill.blackhole.CanUseSkill())
                        skillUsed = true;
                }

                break;
            }
        }

        if (Player.skill.blackhole.SkillCompleted())
            StateMachine.ChangeState(Player.airState);
    }
}