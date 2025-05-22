using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [SerializeField] private SkillTreeSlotUI blackHoleUnlockButton;
    public bool blackholeUnlocked;
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [SerializeField] private float blackholeDuration;
    [Space] [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    private BlackholeSkillController currentBlackhole;

    private void UnlockBlackhole()
    {
        if (blackHoleUnlockButton.unlocked)
            blackholeUnlocked = true;
    }

    protected override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, Player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackHole.GetComponent<BlackholeSkillController>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown,
            blackholeDuration);

        AudioManager.instance.PlaySfx(18, Player.transform);
        AudioManager.instance.PlaySfx(19, Player.transform);
    }

    protected override void Start()
    {
        base.Start();

        blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockBlackhole();
    }
}