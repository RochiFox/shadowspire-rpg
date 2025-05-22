using UnityEngine;

public class DeathBringerSpellController : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;

    private CharacterStats myStats;

    private readonly Collider2D[] animationsResult = new Collider2D[10];

    public void SetupSpell(CharacterStats _stats) => myStats = _stats;

    private void AnimationTrigger()
    {
        int size = Physics2D.OverlapBoxNonAlloc(check.position, boxSize, whatIsPlayer, animationsResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = animationsResult[i];

            if (hit.GetComponent<Player>())
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                myStats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }

    private void OnDrawGizmos() => Gizmos.DrawWireCube(check.position, boxSize);

    private void SelfDestroy() => Destroy(gameObject);
}