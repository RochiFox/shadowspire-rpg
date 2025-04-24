using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBringerTriggers : EnemyAnimationTriggers
{
    private EnemyDeathBringer enemyDeathBringer => GetComponentInParent<EnemyDeathBringer>();

    private void Relocate() => enemyDeathBringer.FindPosition();

    private void MakeInvisible() => enemyDeathBringer.fx.MakeTransparent(true);
    private void MakeVisible() => enemyDeathBringer.fx.MakeTransparent(false);
}
