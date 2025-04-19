using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirectionPath = "";
    private string dataFileName = "";

    public FileDataHandler(string _dataDirectionPath, string _dataFileName)
    {
        dataDirectionPath = _dataDirectionPath;
        dataFileName = _dataFileName;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirectionPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            File.WriteAllText(fullPath, dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirectionPath, dataFileName);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = File.ReadAllText(fullPath);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirectionPath, dataFileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
