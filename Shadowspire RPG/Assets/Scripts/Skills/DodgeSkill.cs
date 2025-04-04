using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private SkillTreeSlotUI unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private SkillTreeSlotUI unlockMirageButton;
    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    public void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    public void UnlockMirageDodge()
    {
        if (unlockMirageButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDirection, 0));
        }
    }
}
