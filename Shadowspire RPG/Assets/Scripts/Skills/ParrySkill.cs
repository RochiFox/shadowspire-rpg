using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")] [SerializeField] private SkillTreeSlotUI parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")] [SerializeField]
    private SkillTreeSlotUI restoreUnlockButton;

    [Range(0f, 1f)] [SerializeField] private float restoreHealthPercentage;
    private bool restoreUnlocked { get; set; }

    [Header("Parry with mirage")] [SerializeField]
    private SkillTreeSlotUI parryWithMirageUnlockButton;

    private bool parryWithMirageUnlocked { get; set; }

    protected override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(Player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            Player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
            parryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
            parryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
    }
}