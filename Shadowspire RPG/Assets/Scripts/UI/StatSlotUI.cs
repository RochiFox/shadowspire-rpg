using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea] [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText)
            statNameText.text = statName;
    }

    public void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();

            if (statType == StatType.Health)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if (statType == StatType.Damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.CritPower)
                statValueText.text = (playerStats.critPower.GetValue() + playerStats.strength.GetValue()).ToString();

            if (statType == StatType.CritChance)
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.Evasion)
                statValueText.text = (playerStats.evasion.GetValue() + playerStats.agility.GetValue()).ToString();

            if (statType == StatType.MagicRes)
                statValueText.text =
                    (playerStats.magicResistance.GetValue() + (playerStats.intelligence.GetValue() * 3)).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}