using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    private RectTransform myTransform => GetComponent<RectTransform>();
    private Slider slider;

    private void Start()
    {
        slider = GetComponentInChildren<Slider>();

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void OnEnable()
    {
        entity.OnFlipped += FlipUI;
        myStats.OnHealthChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (entity)
            entity.OnFlipped -= FlipUI;

        if (myStats)
            myStats.OnHealthChanged -= UpdateHealthUI;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);
}