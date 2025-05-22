using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")] [SerializeField] private SkillTreeSlotUI unlockDodgeButton;
    [SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")] [SerializeField]
    private SkillTreeSlotUI unlockMirageDodge;

    public bool dodgeMirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            Player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (unlockMirageDodge.unlocked)
            dodgeMirageUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
            SkillManager.instance.clone.CreateClone(Player.transform, new Vector3(2 * Player.facingDir, 0));
    }
}