using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button saveAndExitButton;

    private void Awake()
    {
        saveAndExitButton.onClick.AddListener(OnSaveAndExit);
    }

    private void OnSaveAndExit()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.SaveGame();
        }

        Application.Quit();
    }
}