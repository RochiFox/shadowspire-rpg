using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")] [SerializeField] private SkillTreeSlotUI dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on dash")] [SerializeField]
    private SkillTreeSlotUI cloneOnDashUnlockButton;

    private bool cloneOnDashUnlocked { get; set; }

    [Header("Clone on arrival")] [SerializeField]
    private SkillTreeSlotUI cloneOnArrivalUnlockButton;

    private bool cloneOnArrivalUnlocked { get; set; }

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }

    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
            SkillManager.instance.clone.CreateClone(Player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (cloneOnArrivalUnlocked)
            SkillManager.instance.clone.CreateClone(Player.transform, Vector3.zero);
    }
}