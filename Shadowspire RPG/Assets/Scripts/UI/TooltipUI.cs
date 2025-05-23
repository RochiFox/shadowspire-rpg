using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private float xLimit = 600;
    [SerializeField] private float yLimit = 280;

    [SerializeField] private float xOffset = 100;
    [SerializeField] private float yOffset = 100;

    protected virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset;
        float newYOffset;

        if (mousePosition.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }

    protected static void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
            _text.fontSize *= .8f;
    }
}