using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    private CharacterStats stats;
    private int arrowFacingDir = 1;

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);

        if (arrowFacingDir == 1 && rb.velocity.x < 0)
        {
            arrowFacingDir = -1;
            sr.flipX = true;
        }
    }

    public void SetupArrow(float _speed, CharacterStats _stats)
    {
        sr = GetComponent<SpriteRenderer>();
        xVelocity = _speed;
        stats = _stats;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.GetComponent<CharacterStats>()?.isInvincible == true)
            return;

        if (_collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            stats.DoDamage(_collision.GetComponent<CharacterStats>());

            if (targetLayerName == "Enemy")
                Destroy(gameObject);

            StuckInto(_collision);
        }
        else if (_collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            StuckInto(_collision);
    }

    private void StuckInto(Collider2D _collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = _collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void FlipArrow()
    {
        if (flipped)
            return;

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);
        targetLayerName = "Enemy";
    }
}