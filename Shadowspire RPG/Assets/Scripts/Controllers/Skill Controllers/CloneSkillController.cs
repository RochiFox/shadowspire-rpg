using System.Collections;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private static readonly int AttackNumber = Animator.StringToHash("AttackNumber");

    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate;

    [Space] [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float closestEnemyCheckRadius = 25;
    [SerializeField] private Transform closestEnemy;

    private readonly Collider2D[] attackResult = new Collider2D[10];
    private readonly Collider2D[] closestEnemyResult = new Collider2D[10];

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        StartCoroutine(FaceClosestTarget());
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset,
        bool _canDuplicate, float _chanceToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
            anim.SetInteger(AttackNumber, Random.Range(1, 3));

        attackMultiplier = _attackMultiplier;
        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        canDuplicateClone = _canDuplicate;
        chanceToDuplicate = _chanceToDuplicate;
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        int size = Physics2D.OverlapCircleNonAlloc(attackCheck.position, attackCheckRadius, attackResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = attackResult[i];

            if (hit.GetComponent<Enemy>())
            {
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>()); // make a new function for clone damage to regulate damage;

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultiplier);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData)
                        weaponData.Effect(hit.transform);
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private IEnumerator FaceClosestTarget()
    {
        yield return null;

        FindClosestEnemy();

        if (closestEnemy)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }

    private void FindClosestEnemy()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, closestEnemyCheckRadius, closestEnemyResult,
            whatIsEnemy);

        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < size; i++)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, closestEnemyResult[i].transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = closestEnemyResult[i].transform;
            }
        }
    }
}