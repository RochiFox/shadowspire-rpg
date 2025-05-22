using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Player>() && AudioManager.instance)
            AudioManager.instance.PlaySfx(areaSoundIndex, null);
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Player>() && AudioManager.instance)
            AudioManager.instance.StopSfxWithTime(areaSoundIndex);
    }
}