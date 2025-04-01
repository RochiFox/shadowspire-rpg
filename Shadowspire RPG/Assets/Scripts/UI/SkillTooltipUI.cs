using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillText;

    public void ShowTooltip(string _skillDescription, string _skillName)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        gameObject.SetActive(true);
    }

    public void HideTooltip() => gameObject.SetActive(false);
}
