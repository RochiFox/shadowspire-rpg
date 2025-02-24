using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));
        }
    }

    public void SetupClone(Transform newTransform, float cloneDuration)
    {
        transform.position = newTransform.position;

        cloneTimer = cloneDuration;
    }
}
