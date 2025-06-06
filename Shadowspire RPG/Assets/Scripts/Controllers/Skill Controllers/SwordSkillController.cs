using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private static readonly int Rotation = Animator.StringToHash("Rotation");

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;
    private float returnSpeed = 12;

    [Header("Pierce info")] private float pierceAmount;

    [Header("Bounce info")] private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Spin info")] private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    private readonly Collider2D[] spinResult = new Collider2D[10];
    private readonly Collider2D[] targetResult = new Collider2D[10];

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration,
        float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            anim.SetBool(Rotation, true);


        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke(nameof(DestroyMe), 7);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;


        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position,
                returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    int size = Physics2D.OverlapCircleNonAlloc(transform.position, 1, spinResult);

                    for (int i = 0; i < size; i++)
                    {
                        Collider2D hit = spinResult[i];

                        if (hit.GetComponent<Enemy>())
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position,
                bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                {
                    SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                    targetIndex++;
                    bounceAmount--;

                    if (bounceAmount <= 0)
                    {
                        isBouncing = false;
                        isReturning = true;
                    }

                    if (targetIndex >= enemyTarget.Count)
                        targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (isReturning)
            return;

        if (_collision.GetComponent<Enemy>())
        {
            Enemy enemy = _collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupTargetsForBounce(_collision);
        StuckInto(_collision);
    }

    private void SwordSkillDamage(Enemy _enemy)
    {
        EnemyStats enemyStats = _enemy.GetComponent<EnemyStats>();

        player.stats.DoDamage(enemyStats);

        if (player.skill.sword.timeStopUnlocked)
            _enemy.FreezeTimeFor(freezeTimeDuration);

        if (player.skill.sword.vulnerableUnlocked)
            enemyStats.MakeVulnerableFor(freezeTimeDuration);

        ItemDataEquipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equippedAmulet)
            equippedAmulet.Effect(_enemy.transform);
    }

    private void SetupTargetsForBounce(Collider2D _collision)
    {
        if (_collision.GetComponent<Enemy>())
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                int size = Physics2D.OverlapCircleNonAlloc(transform.position, 10, targetResult);

                for (int i = 0; i < size; i++)
                {
                    Collider2D hit = targetResult[i];

                    if (hit.GetComponent<Enemy>())
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private bool spinWasTriggered;

    private void StuckInto(Collider2D _collision)
    {
        if (_collision.GetComponent<CharacterStats>()?.isInvincible == true)
            return;

        if (pierceAmount > 0 && _collision.GetComponent<Enemy>())
        {
            pierceAmount--;
            return;
        }

        if (isSpinning && !spinWasTriggered)
        {
            spinWasTriggered = true;
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponentInChildren<ParticleSystem>().Play();

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool(Rotation, false);
        transform.parent = _collision.transform;
    }
}