using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")] [SerializeField]
    private SkillTreeSlotUI unlockCloneInsteadButton;

    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Crystal simple")] [SerializeField]
    private SkillTreeSlotUI unlockCrystalButton;

    public bool crystalUnlocked { get; private set; }

    [Header("Explosive crystal")] [SerializeField]
    private SkillTreeSlotUI unlockExplosiveButton;

    [SerializeField] private float explosiveCooldown;
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")] [SerializeField]
    private SkillTreeSlotUI unlockMovingCrystalButton;

    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")] [SerializeField]
    private SkillTreeSlotUI unlockMultiStackButton;

    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
    }

    #region Unlock skill region

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockCrystalMirage();
        UnlockExplosiveCrystal();
        UnlockMovingCrystal();
        UnlockMultiStack();
    }

    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
            crystalUnlocked = true;
    }

    private void UnlockCrystalMirage()
    {
        if (unlockCloneInsteadButton.unlocked)
            cloneInsteadOfCrystal = true;
    }

    private void UnlockExplosiveCrystal()
    {
        if (unlockExplosiveButton.unlocked)
        {
            canExplode = true;
            cooldown = explosiveCooldown;
        }
    }

    private void UnlockMovingCrystal()
    {
        if (unlockMovingCrystalButton.unlocked)
            canMoveToEnemy = true;
    }

    private void UnlockMultiStack()
    {
        if (unlockMovingCrystalButton.unlocked)
            canUseMultiStacks = true;
    }

    #endregion

    protected override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if (!currentCrystal)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
                return;

            Vector2 playerPos = Player.transform.position;
            Player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, Player.transform.position, Quaternion.identity);
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
            FindClosestEnemy(currentCrystal.transform), Player);
    }

    public void CurrentCrystalChooseRandomTarget() =>
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke(nameof(ResetAbility), useTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[^1];
                GameObject newCrystal = Instantiate(crystalToSpawn, Player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(crystalDuration, canExplode,
                    canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), Player);

                if (crystalLeft.Count <= 0)
                {
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }

                return true;
            }
        }

        return false;
    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}