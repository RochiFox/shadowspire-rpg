using UnityEngine;

public class EnemyShady : Enemy
{
    [Header("Shady specifics")] public float battleStateMoveSpeed;

    [SerializeField] private GameObject explosivePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxSize;

    #region States

    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    private ShadyDeadState deadState { get; set; }
    private ShadyStunnedState stunnedState { get; set; }
    public ShadyBattleState battleState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);

        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);

        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public override void AnimationSpecialAttackTrigger()
    {
        GameObject newExplosive = Instantiate(explosivePrefab, attackCheck.position, Quaternion.identity);
        newExplosive.GetComponent<ExplosiveController>().SetupExplosive(stats, growSpeed, maxSize, attackCheckRadius);

        cd.enabled = false;
        rb.gravityScale = 0;
    }

    public void SelfDestroy() => Destroy(gameObject);
}