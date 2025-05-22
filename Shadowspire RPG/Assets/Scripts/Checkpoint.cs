using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static readonly int Active = Animator.StringToHash("active");

    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Player>())
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()
    {
        if (activationStatus == false)
            AudioManager.instance.PlaySfx(4, transform);

        activationStatus = true;
        anim.SetBool(Active, true);
    }
}