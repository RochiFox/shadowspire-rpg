using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName; 

    private GameData gameData;
    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete save file")]
    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        dataHandler.Delete();
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    void Start()
    {
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        if (dataHandler == null)
        {
            return false;
        }

        if (dataHandler.Load() != null)
        {
            return true;
        }

        return false;
    }

    public void SaveAndExit()
    {
        SaveGame();
        Application.Quit();
    }
}
