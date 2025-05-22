using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Player>())
        {
            if (_collision.GetComponent<CharacterStats>().isDead)
                return;

            myItemObject.PickupItem();
        }
    }
}