using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;

    private readonly Collider2D[] effectsResult = new Collider2D[10];

    public override void ExecuteEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.1f)
            return;

        if (!Inventory.instance.CanUseArmor())
            return;

        int size = Physics2D.OverlapCircleNonAlloc(_transform.position, 2, effectsResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = effectsResult[i];

            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}