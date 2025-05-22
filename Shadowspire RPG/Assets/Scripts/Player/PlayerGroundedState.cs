using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(
        _player, _stateMachine, _animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && Player.skill.blackhole.blackholeUnlocked)
        {
            if (Player.skill.blackhole.cooldownTimer > 0)
            {
                Player.fx.CreatePopUpText("Cooldown!");
                return;
            }

            StateMachine.ChangeState(Player.blackHole);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && Player.skill.sword.swordUnlocked)
            StateMachine.ChangeState(Player.aimSword);

        if (Input.GetKeyDown(KeyCode.Q) && Player.skill.parry.parryUnlocked)
        {
            if (Player.skill.parry.cooldownTimer > 0)
            {
                return;
            }

            StateMachine.ChangeState(Player.counterAttack);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            StateMachine.ChangeState(Player.primaryAttack);

        if (!Player.IsGroundDetected())
            StateMachine.ChangeState(Player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && Player.IsGroundDetected())
            StateMachine.ChangeState(Player.jumpState);
    }

    private bool HasNoSword()
    {
        if (!Player.sword)
        {
            return true;
        }

        Player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}