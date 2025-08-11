using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField]
    private FadeScreenUI fadeScreen;

    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space][SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject councilUI;
    [SerializeField] private GameObject councilHints;

    public SkillTooltipUI skillToolTip;
    public ItemTooltipUI itemToolTip;
    public StatTooltipUI statToolTip;
    public CraftWindowUI craftWindow;

    [SerializeField] private VolumeSliderUI[] volumeSettings;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.E))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKeyTo(optionsUI);

        if (Input.GetKeyDown(KeyCode.H) && inGameUI.activeSelf)
            councilHints.SetActive(!councilHints.activeSelf);
    }

    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            bool fadeScreenUi = child.GetComponent<FadeScreenUI>();
            bool isCouncilUI = child.gameObject == councilUI;

            if (!fadeScreenUi && !isCouncilUI)
                child.gameObject.SetActive(false);
        }

        if (_menu)
        {
            AudioManager.instance.PlaySfx(5, null);
            _menu.SetActive(true);
        }

        if (GameManager.instance)
        {
            bool isGamePaused = _menu != inGameUI;
            GameManager.PauseGame(isGamePaused);

            AudioManager.instance.StopAllSfx(isGamePaused);
        }
    }

    private void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf &&
                !transform.GetChild(i).GetComponent<FadeScreenUI>())
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
    }

    private IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (VolumeSliderUI item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (VolumeSliderUI item in volumeSettings)
        {
            if (!_data.volumeSettings.ContainsKey(item.parameter))
                _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}