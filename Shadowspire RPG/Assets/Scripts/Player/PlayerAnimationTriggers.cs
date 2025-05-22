using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private readonly Collider2D[] triggersResult = new Collider2D[10];

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySfx(2, null);

        int size = Physics2D.OverlapCircleNonAlloc(player.attackCheck.position, player.attackCheckRadius,
            triggersResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = triggersResult[i];

            if (hit.GetComponent<Enemy>())
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();

                if (target)
                    player.stats.DoDamage(target);

                ItemDataEquipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData)
                    weaponData.Effect(target.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}