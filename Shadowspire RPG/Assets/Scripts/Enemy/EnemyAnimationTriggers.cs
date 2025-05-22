using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();

    private readonly Collider2D[] attackResult = new Collider2D[10];

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        int size = Physics2D.OverlapCircleNonAlloc(enemy.attackCheck.position, enemy.attackCheckRadius, attackResult);

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = attackResult[i];

            if (hit.GetComponent<Player>())
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    protected void SpecialAttackTrigger()
    {
        enemy.AnimationSpecialAttackTrigger();
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}