using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystlalInsteadOfClone;

    public void CreateClone(Transform clonePosition, Vector3 _offset)
    {
        if (crystlalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDirection, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);

        CreateClone(_transform, _offset);
    }
}
