using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    public int currency;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Player>())
        {
            PlayerManager.instance.currency += currency;
            Destroy(this.gameObject);
        }
    }
}