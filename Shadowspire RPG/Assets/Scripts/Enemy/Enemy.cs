using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public EnemyStateMachine stateMachine { get; private set; }

    void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }

    void Update()
    {
        stateMachine.currentState.Update();
    }
}
