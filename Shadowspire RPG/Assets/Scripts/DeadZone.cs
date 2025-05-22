using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<CharacterStats>())
            _collision.GetComponent<CharacterStats>().KillEntity();
        else
            Destroy(_collision.gameObject);
    }
}