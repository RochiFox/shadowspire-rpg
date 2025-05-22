using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private static readonly int Explode = Animator.StringToHash("Explode");

    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private bool canGrow;
    private const float GROW_SPEED = 5;

    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;

    private readonly Collider2D[] randomEnemyResult = new Collider2D[10];
    private readonly Collider2D[] animationsResult = new Collider2D[10];

    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed,
        Transform _closestTarget, Player _player)
    {
        player = _player;
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();

        int size = Physics2D.OverlapCircleNonAlloc(transform.position, radius, randomEnemyResult, whatIsEnemy);

        if (size > 0)
            closestTarget = randomEnemyResult[Random.Range(0, size)].transform;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            if (!closestTarget)
                return;

            transform.position =
                Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                FinishCrystal();
                canMove = false;
            }
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), GROW_SPEED * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, cd.radius, animationsResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = animationsResult[i];

            if (hit.GetComponent<Enemy>())
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemDataEquipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

                if (equippedAmulet)
                    equippedAmulet.Effect(hit.transform);
            }
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger(Explode);
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}