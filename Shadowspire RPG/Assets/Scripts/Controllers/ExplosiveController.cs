using UnityEngine;

public class ExplosiveController : MonoBehaviour
{
    private static readonly int Explode = Animator.StringToHash("Explode");

    private Animator anim;
    private CharacterStats myStats;
    private float growSpeed = 15;
    private float maxSize = 6;
    private float explosionRadius;

    private bool canGrow = true;

    private readonly Collider2D[] animationExplodeResult = new Collider2D[10];

    private void Update()
    {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize),
                growSpeed * Time.deltaTime);

        if (maxSize - transform.localScale.x < .5f)
        {
            canGrow = false;
            anim.SetTrigger(Explode);
        }
    }

    public void SetupExplosive(CharacterStats _myStats, float _growSpeed, float _maxSize, float _radius)
    {
        anim = GetComponent<Animator>();

        myStats = _myStats;
        growSpeed = _growSpeed;
        maxSize = _maxSize;
        explosionRadius = _radius;
    }

    private void AnimationExplodeEvent()
    {
        int size = Physics2D.OverlapCircleNonAlloc(transform.position, explosionRadius, animationExplodeResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = animationExplodeResult[i];

            if (hit.GetComponent<CharacterStats>())
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Player>()?.fx.ScreenShake(new Vector3(2, 2));
            }
        }
    }

    private void SelfDestroy() => Destroy(gameObject);
}