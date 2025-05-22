using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;

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