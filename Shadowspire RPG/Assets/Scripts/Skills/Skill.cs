using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    public float cooldownTimer;

    protected Player Player;

    private readonly Collider2D[] enemyResult = new Collider2D[10];

    protected virtual void Start()
    {
        Player = PlayerManager.instance.player;

        CheckUnlock();
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        Player.fx.CreatePopUpText("Cooldown");
        return false;
    }

    protected virtual void UseSkill()
    {
        // do some skill specific things
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        int size = Physics2D.OverlapCircleNonAlloc(_checkTransform.position, 25, enemyResult);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        for (int i = 0; i < size; i++)
        {
            Collider2D hit = enemyResult[i];

            if (hit.GetComponent<Enemy>())
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}