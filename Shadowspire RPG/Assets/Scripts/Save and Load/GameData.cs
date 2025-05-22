using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, bool> skillTree = new();
    public SerializableDictionary<string, int> inventory = new();
    public List<string> equipmentId = new();

    public SerializableDictionary<string, bool> checkpoints = new();
    public string closestCheckpointId = string.Empty;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;
    public SerializableDictionary<string, float> volumeSettings = new();
}