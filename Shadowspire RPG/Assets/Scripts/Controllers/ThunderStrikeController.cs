using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Enemy>())
        {
            PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyTarget = _collision.GetComponent<EnemyStats>();
            playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}