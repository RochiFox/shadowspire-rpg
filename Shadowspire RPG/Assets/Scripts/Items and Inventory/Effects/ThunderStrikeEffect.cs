using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);
        Destroy(newThunderStrike, 1);
    }
}