using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDirection;
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);

        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        newSwordScript.SetupSword(launchDirection, swordGravity);
    }
}
