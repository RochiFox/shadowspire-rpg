using System.Collections;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CritChance,
    CritPower,
    Health,
    Armor,
    Evasion,
    MagicRes,
    FireDamage,
    IceDamage,
    LightingDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")] public Stat strength; // 1 point increase damage by 1 and crit.power by 1%
    public Stat agility; // 1 point increase evasion by 1% and crit.chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 5 points

    [Header("Offensive stats")] public Stat damage;
    public Stat critChance;
    public Stat critPower; // default value 150%

    [Header("Defensive stats")] public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")] public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited; // does damage over time
    public bool isChilled; // reduce armor by 20%
    public bool isShocked; // reduce accuracy by 20%

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private const float IGNITE_DAMAGE_COOLDOWN = .3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;
    public int currentHealth;

    public System.Action OnHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    private bool isVulnerable;

    private readonly Collider2D[] hitTargetResult = new Collider2D[10];

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (isIgnited)
            ApplyIgniteDamage();
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCoroutine(_duration));

    private IEnumerator VulnerableCoroutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private static IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (_targetStats.isInvincible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        fx.CreateHitFx(_targetStats.transform, criticalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
    }

    #region Magical damage and ailemnts

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int fireDmg = fireDamage.GetValue();
        int iceDmg = iceDamage.GetValue();
        int lightingDmg = lightingDamage.GetValue();

        int totalMagicalDamage = fireDmg + iceDmg + lightingDmg + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(fireDmg, iceDmg, lightingDmg) <= 0)
            return;

        AttemptToApplyAilments(_targetStats, fireDmg, iceDmg, lightingDmg);
    }

    private static void AttemptToApplyAilments(CharacterStats _targetStats, int _fireDamage, int _iceDamage,
        int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        switch (canApplyIgnite)
        {
            case false when !canApplyChill && !canApplyShock:
                switch (Random.value)
                {
                    case < .3f when _fireDamage > 0:
                        _targetStats.ApplyAilments(true, false, false);
                        return;
                    case < .5f when _iceDamage > 0:
                        _targetStats.ApplyAilments(false, true, false);
                        return;
                    case < .5f when _lightingDamage > 0:
                        _targetStats.ApplyAilments(false, false, true);
                        return;
                }

                break;
            case true:
                _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
                break;
        }

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;


        if (_ignite && canApplyIgnite)
        {
            isIgnited = true;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }

        if (_chill && canApplyChill)
        {
            chilledTimer = ailmentsDuration;
            isChilled = true;

            const float slowPercentage = .2f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFxFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(true);
            }
            else
            {
                if (GetComponent<Player>())
                    return;

                HitNearestTargetWithShockStrike();
            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        shockedTimer = ailmentsDuration;
        isShocked = _shock;

        fx.ShockFxFor(ailmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, 25, hitTargetResult);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = hitTargetResult[i];

            if (hit.GetComponent<Enemy>() && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (!closestEnemy)
                closestEnemy = transform;
        }

        if (closestEnemy)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrikeController>()
                .Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    private void ApplyIgniteDamage()
    {
        if (igniteDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = IGNITE_DAMAGE_COOLDOWN;
        }
    }

    private void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    private void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    #endregion

    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (OnHealthChanged != null)
            OnHealthChanged();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString());

        OnHealthChanged?.Invoke();
    }

    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    #region Stat calculations

    protected static int CheckTargetArmor(CharacterStats _targetStats, int _totalDamage)
    {
        if (_targetStats.isChilled)
            _totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            _totalDamage -= _targetStats.armor.GetValue();


        _totalDamage = Mathf.Clamp(_totalDamage, 0, int.MaxValue);
        return _totalDamage;
    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int _totalMagicalDamage)
    {
        _totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        _totalMagicalDamage = Mathf.Clamp(_totalMagicalDamage, 0, int.MaxValue);
        return _totalMagicalDamage;
    }

    protected virtual void OnEvasion()
    {
    }

    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        return Random.Range(0, 100) <= totalCriticalChance;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #endregion

    public Stat GetStat(StatType _statType)
    {
        return _statType switch
        {
            StatType.Strength => strength,
            StatType.Agility => agility,
            StatType.Intelligence => intelligence,
            StatType.Vitality => vitality,
            StatType.Damage => damage,
            StatType.CritChance => critChance,
            StatType.CritPower => critPower,
            StatType.Health => maxHealth,
            StatType.Armor => armor,
            StatType.Evasion => evasion,
            StatType.MagicRes => magicResistance,
            StatType.FireDamage => fireDamage,
            StatType.IceDamage => iceDamage,
            StatType.LightingDamage => lightingDamage,
            _ => null
        };
    }
}