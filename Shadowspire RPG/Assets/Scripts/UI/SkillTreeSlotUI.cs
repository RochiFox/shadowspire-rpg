using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;
    private Image skillImage;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea] [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool unlocked;

    [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlotUI - " + skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UnlockSkillSlot);
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();

        skillImage.color = lockedSkillColor;

        if (unlocked)
            skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
            return;

        if (shouldBeUnlocked.Any(_skill => _skill.unlocked == false))
        {
            return;
        }

        if (shouldBeLocked.Any(_skill => _skill.unlocked))
        {
            return;
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName, skillCost);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.Remove(skillName, out bool _))
        {
            _data.skillTree.Add(skillName, unlocked);
        }
        else
            _data.skillTree.Add(skillName, unlocked);
    }
}